using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource menuMusicSource;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        volumeSlider.value = savedVolume;

        if (menuMusicSource != null)
        {
            menuMusicSource.volume = savedVolume;
        }
    }

    public void OnVolumeChanged()
    {
        float volume = volumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

        if (menuMusicSource != null)
        {
            menuMusicSource.volume = volume;
        }
    }
}