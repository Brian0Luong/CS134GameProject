using UnityEngine;

public class FootstepAnimation : MonoBehaviour
{
    [Header("Components")]
    public AudioSource audioSource;
    public PlayerWaterState playerWaterState;

    [Header("Footstep Sounds")]
    public AudioClip[] walkClips;
    public AudioClip[] runClips;

    [Header("Settings")]
    public float minStepInterval = 0.15f;

    [Header("Above Water Audio")]
    public float normalPitch = 1f;
    public float normalVolume = 1f;

    [Header("Underwater Audio")]
    public float underwaterPitch = 0.9f;
    public float underwaterVolume = 0.85f;

    private float lastStepTime = -999f;

    public void PlayWalkFootstep()
    {
        if (Time.time - lastStepTime < minStepInterval) return;
        if (audioSource == null || walkClips == null || walkClips.Length == 0) return;

        lastStepTime = Time.time;
        ApplyFootstepSettings();

        int index = Random.Range(0, walkClips.Length);
        audioSource.PlayOneShot(walkClips[index], 1f);
    }

    public void PlayRunFootstep()
    {
        if (Time.time - lastStepTime < minStepInterval) return;
        if (audioSource == null || runClips == null || runClips.Length == 0) return;

        lastStepTime = Time.time;
        ApplyFootstepSettings();

        int index = Random.Range(0, runClips.Length);
        audioSource.PlayOneShot(runClips[index], 1f);
    }

    void ApplyFootstepSettings()
    {
        bool underwater = playerWaterState != null && playerWaterState.IsUnderwater;

        if (underwater)
        {
            audioSource.pitch = underwaterPitch;
            audioSource.volume = underwaterVolume;
        }
        else
        {
            audioSource.pitch = normalPitch;
            audioSource.volume = normalVolume;
        }
    }
}