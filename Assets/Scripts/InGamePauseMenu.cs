using UnityEngine;
using UnityEngine.UI;

public class InGamePauseMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    [Header("Sliders")]
    public Slider musicVolumeSlider;
    public Slider masterVolumeSlider;

    [Header("Audio")]
    public AudioSource bgmSource;

    private bool isOpen = false;

    private void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        musicVolumeSlider.value = savedMusicVolume;
        masterVolumeSlider.value = savedMasterVolume;

        if (bgmSource != null)
        {
            bgmSource.volume = savedMusicVolume;
        }

        AudioListener.volume = savedMasterVolume;

        settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        isOpen = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        isOpen = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMusicVolumeChanged()
    {
        float volume = musicVolumeSlider.value;

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    public void OnMasterVolumeChanged()
    {
        float volume = masterVolumeSlider.value;

        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();

        AudioListener.volume = volume;
    }
}