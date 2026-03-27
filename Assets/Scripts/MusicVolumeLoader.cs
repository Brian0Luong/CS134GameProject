using UnityEngine;

public class MusicVolumeLoader : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        audioSource.volume = savedVolume;
    }
}