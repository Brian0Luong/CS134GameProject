using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class PlayerWaterState : MonoBehaviour
{
    [Header("References")]
    public Transform headPoint;
    public WaterRiser currentWater;
    public Slider oxygenBar;

    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float oxygenDrainPerSecond = 30f;
    public float oxygenRecoverPerSecond = 40f;

    [Header("Lose Settings")]
    public bool reloadSceneOnLose = true;

    private float currentOxygen;
    private bool hasLost = false;
    public Volume underwaterVolume;

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
    }

    void Update()
    {
        if (hasLost) return;
        if (headPoint == null || currentWater == null) return;

        bool underwater = headPoint.position.y < currentWater.CurrentWaterY;

        if (underwater)
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

    public void SetCurrentWater(WaterRiser water)
    {
        currentWater = water;
    }

    void UpdateUI()
    {
        if (oxygenBar == null) return;

        oxygenBar.value = currentOxygen;

        if (currentOxygen < maxOxygen)
        {
            oxygenBar.gameObject.SetActive(true);
        }
        else
        {
            oxygenBar.gameObject.SetActive(false);
        }
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
