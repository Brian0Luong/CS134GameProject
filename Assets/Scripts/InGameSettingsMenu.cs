using UnityEngine;
using UnityEngine.UI;

public class InGameSettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public AudioSource bgmSource;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        volumeSlider.value = savedVolume;

        if (bgmSource != null)
        {
            bgmSource.volume = savedVolume;
        }

        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnVolumeChanged()
    {
        float volume = volumeSlider.value;

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }
}