using UnityEngine;

public class FootstepAnimation : MonoBehaviour
{
    [Header("Components")]
    public AudioSource audioSource;

    [Header("Footstep Sounds")]
    public AudioClip[] walkClips;
    public AudioClip[] runClips;

    [Header("Settings")]
    public float minStepInterval = 0.15f;

    private float lastStepTime = -999f;

    public void PlayWalkFootstep()
    {
        if (Time.time - lastStepTime < minStepInterval) return;
        if (audioSource == null || walkClips == null || walkClips.Length == 0) return;

        lastStepTime = Time.time;
        audioSource.pitch = 1f;

        int index = Random.Range(0, walkClips.Length);
        audioSource.PlayOneShot(walkClips[index]);
    }

    public void PlayRunFootstep()
    {
        if (Time.time - lastStepTime < minStepInterval) return;
        if (audioSource == null || runClips == null || runClips.Length == 0) return;

        lastStepTime = Time.time;
        audioSource.pitch = 1f;

        int index = Random.Range(0, runClips.Length);
        audioSource.PlayOneShot(runClips[index]);
    }
}