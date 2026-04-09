using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class InGamePauseMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject audioPanel;
    public GameObject controlsPanel;

    [Header("Sliders")]
    public Slider musicVolumeSlider;
    public Slider masterVolumeSlider;
    public Slider mouseXSensitivitySlider;
    public Slider mouseYSensitivitySlider;

    [Header("Camera")]
    public CinemachineFreeLook freeLookCamera;

    [Header("Mouse Sensitivity Ranges")]
    public float minMouseXSpeed = 100f;
    public float maxMouseXSpeed = 1000f;
    public float minMouseYSpeed = 0.5f;
    public float maxMouseYSpeed = 10f;

    [Header("Audio")]
    public AudioSource bgmSource;

    [Header("Default Mouse Sensitivity")]
    public float defaultMouseXSpeed = 700f;
    public float defaultMouseYSpeed = 4f;

    private bool isOpen = false;

    private void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        float savedMouseXSpeed = PlayerPrefs.GetFloat("MouseXSpeed", defaultMouseXSpeed);
        float savedMouseYSpeed = PlayerPrefs.GetFloat("MouseYSpeed", defaultMouseYSpeed);

        musicVolumeSlider.value = savedMusicVolume;
        masterVolumeSlider.value = savedMasterVolume;

        if (mouseXSensitivitySlider != null)
        {
            mouseXSensitivitySlider.minValue = minMouseXSpeed;
            mouseXSensitivitySlider.maxValue = maxMouseXSpeed;
            mouseXSensitivitySlider.value = savedMouseXSpeed;
        }

        if (mouseYSensitivitySlider != null)
        {
            mouseYSensitivitySlider.minValue = minMouseYSpeed;
            mouseYSensitivitySlider.maxValue = maxMouseYSpeed;
            mouseYSensitivitySlider.value = savedMouseYSpeed;
        }

        if (bgmSource != null)
        {
            bgmSource.volume = savedMusicVolume;
        }

        AudioListener.volume = savedMasterVolume;

        ApplyMouseSensitivity(savedMouseXSpeed, savedMouseYSpeed);

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
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
        isOpen = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenAudioPanel()
    {
        settingsPanel.SetActive(false);
        audioPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        settingsPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackToSettings()
    {
        settingsPanel.SetActive(true);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
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

    public void OnMouseXSensitivityChanged()
    {
        if (mouseXSensitivitySlider == null) return;

        float xSpeed = mouseXSensitivitySlider.value;

        PlayerPrefs.SetFloat("MouseXSpeed", xSpeed);
        PlayerPrefs.Save();

        if (freeLookCamera != null)
            freeLookCamera.m_XAxis.m_MaxSpeed = xSpeed;
    }

    public void OnMouseYSensitivityChanged()
    {
        if (mouseYSensitivitySlider == null) return;

        float ySpeed = mouseYSensitivitySlider.value;

        PlayerPrefs.SetFloat("MouseYSpeed", ySpeed);
        PlayerPrefs.Save();

        if (freeLookCamera != null)
            freeLookCamera.m_YAxis.m_MaxSpeed = ySpeed;
    }

    private void ApplyMouseSensitivity(float xSpeed, float ySpeed)
    {
        if (freeLookCamera == null) return;

        freeLookCamera.m_XAxis.m_MaxSpeed = xSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = ySpeed;
    }

    public void ResetMouseSensitivity()
    {
        float x = defaultMouseXSpeed;
        float y = defaultMouseYSpeed;

        if (mouseXSensitivitySlider != null)
            mouseXSensitivitySlider.value = x;

        if (mouseYSensitivitySlider != null)
            mouseYSensitivitySlider.value = y;

        ApplyMouseSensitivity(x, y);

        PlayerPrefs.SetFloat("MouseXSpeed", x);
        PlayerPrefs.SetFloat("MouseYSpeed", y);
        PlayerPrefs.Save();
    }
}