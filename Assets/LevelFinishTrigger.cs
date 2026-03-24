using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LevelFinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject promptText;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private bool playerInRange = false;
    private bool levelFinished = false;
    private PlayerController playerController;

    private void Start()
    {
        if (promptText != null)
            promptText.SetActive(false);

        if (levelCompleteScreen != null)
            levelCompleteScreen.SetActive(false);
    }

    private void Update()
    {
        if (levelFinished)
            return;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            FinishLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        playerController = other.GetComponent<PlayerController>();

        if (promptText != null)
            promptText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;

        if (promptText != null)
            promptText.SetActive(false);
    }

    private void FinishLevel()
    {
        levelFinished = true;

        promptText.SetActive(false);
        levelCompleteScreen.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}