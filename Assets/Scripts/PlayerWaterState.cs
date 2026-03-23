using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class PlayerWaterState : MonoBehaviour
{
    [Header("References")]
    public Transform headPoint;
    public Transform feetPoint;
    public WaterRiser currentWater;
    public Slider oxygenBar;
    public Volume underwaterVolume;

    [Header("Audio References")]
    public AudioSource underwaterLoopSource;
    public AudioLowPassFilter footstepLowPassFilter;
    public AudioSource splashSource;
    public AudioClip splashClip;

    [Header("Audio Settings")]
    public float normalCutoff = 22000f;
    public float underwaterCutoff = 700f;

    [Header("Splash Settings")]
    public float splashCooldown = 0.2f;

    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float oxygenDrainPerSecond = 30f;
    public float oxygenRecoverPerSecond = 40f;

    [Header("Lose Settings")]
    public bool reloadSceneOnLose = true;

    private float currentOxygen;
    private bool hasLost = false;

    private bool wasHeadUnderwater = false;
    private bool wasFeetUnderwater = false;
    private float lastSplashTime = -999f;

    public bool IsUnderwater { get; private set; }

    void Start()
    {
        currentOxygen = maxOxygen;

        if (oxygenBar != null)
        {
            oxygenBar.minValue = 0f;
            oxygenBar.maxValue = maxOxygen;
            oxygenBar.value = currentOxygen;
            oxygenBar.gameObject.SetActive(false);
        }

        if (underwaterVolume != null)
            underwaterVolume.weight = 0f;

        if (underwaterLoopSource != null && underwaterLoopSource.isPlaying)
            underwaterLoopSource.Stop();

        if (footstepLowPassFilter != null)
            footstepLowPassFilter.cutoffFrequency = normalCutoff;
    }

    void Update()
    {
        if (hasLost) return;
        if (headPoint == null || feetPoint == null || currentWater == null) return;

        float waterY = currentWater.CurrentWaterY;

        bool headUnderwater = headPoint.position.y < waterY;
        bool feetUnderwater = feetPoint.position.y < waterY;

        if (headUnderwater != wasHeadUnderwater)
        {
            TryPlaySplash();
            wasHeadUnderwater = headUnderwater;
        }

        if (feetUnderwater != wasFeetUnderwater)
        {
            TryPlaySplash();
            wasFeetUnderwater = feetUnderwater;
        }

        if (headUnderwater != IsUnderwater)
        {
            IsUnderwater = headUnderwater;
            ApplyUnderwaterAudioState();
        }

        if (headUnderwater)
        {
            if (underwaterVolume != null)
                underwaterVolume.weight = 1f;

            currentOxygen -= oxygenDrainPerSecond * Time.deltaTime;
            currentOxygen = Mathf.Max(currentOxygen, 0f);

            if (currentOxygen <= 0f)
            {
                Lose();
                return;
            }
        }
        else
        {
            if (underwaterVolume != null)
                underwaterVolume.weight = 0f;

            currentOxygen += oxygenRecoverPerSecond * Time.deltaTime;
            currentOxygen = Mathf.Min(currentOxygen, maxOxygen);
        }

        UpdateUI();
    }

    void TryPlaySplash()
    {
        if (Time.time - lastSplashTime < splashCooldown)
            return;

        PlaySplash();
        lastSplashTime = Time.time;
    }

    void PlaySplash()
    {
        if (splashSource != null && splashClip != null)
        {
            splashSource.pitch = Random.Range(0.9f, 1.1f);
            splashSource.PlayOneShot(splashClip);
        }
    }

    void ApplyUnderwaterAudioState()
    {
        if (IsUnderwater)
        {
            if (underwaterLoopSource != null && !underwaterLoopSource.isPlaying)
                underwaterLoopSource.Play();

            if (footstepLowPassFilter != null)
                footstepLowPassFilter.cutoffFrequency = underwaterCutoff;
        }
        else
        {
            if (underwaterLoopSource != null && underwaterLoopSource.isPlaying)
                underwaterLoopSource.Stop();

            if (footstepLowPassFilter != null)
                footstepLowPassFilter.cutoffFrequency = normalCutoff;
        }
    }

    public void SetCurrentWater(WaterRiser water)
    {
        currentWater = water;
    }

    void UpdateUI()
    {
        if (oxygenBar == null) return;

        oxygenBar.value = currentOxygen;

        if (currentOxygen < maxOxygen)
            oxygenBar.gameObject.SetActive(true);
        else
            oxygenBar.gameObject.SetActive(false);
    }

    void Lose()
    {
        hasLost = true;
        Debug.Log("Out of oxygen - player loses");

        if (reloadSceneOnLose)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}