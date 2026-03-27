using UnityEngine;
using UnityEngine.UI;

public class InGamePauseMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public AudioSource bgmSource;

    private bool isOpen = false;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        volumeSlider.value = savedVolume;

        if (bgmSource != null)
        {
            bgmSource.volume = savedVolume;
        }

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
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
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