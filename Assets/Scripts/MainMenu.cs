using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainButtonsPanel;
    public GameObject tutorialPanel;


    public void StartGame()
    {
        SceneManager.LoadScene("EscapeRoom_01");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        if (mainButtonsPanel != null)
            mainButtonsPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (mainButtonsPanel != null)
            mainButtonsPanel.SetActive(true);
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        if (mainButtonsPanel != null)
            mainButtonsPanel.SetActive(false);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (mainButtonsPanel != null)
            mainButtonsPanel.SetActive(true);
    }


    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}