using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI interactPrompt;

    private InteractableButton currentButton;
    private PlayerController playerController;
    private bool interacting;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (interacting)
            return;

        if (currentButton != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Interact());
        }
    }

    public void SetInteractable(InteractableButton button)
    {
        currentButton = button;

        if (!interacting && interactPrompt != null)
            interactPrompt.gameObject.SetActive(true);
    }

    public void ClearInteractable(InteractableButton button)
    {
        if (currentButton == button)
        {
            currentButton = null;

            if (interactPrompt != null)
                interactPrompt.gameObject.SetActive(false);
        }
    }

    private IEnumerator Interact()
    {
        if (currentButton == null)
            yield break;

        interacting = true;

        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);

        if (playerController != null)
            playerController.LockMovement();

        Vector3 direction = currentButton.transform.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        animator.ResetTrigger("Interact");
        animator.SetTrigger("Interact");

        yield return null;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("ButtonPress"))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("ButtonPress") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        if (playerController != null)
            playerController.UnlockMovement();

        interacting = false;

        if (currentButton != null && interactPrompt != null)
            interactPrompt.gameObject.SetActive(true);
    }

    // This gets called by the animation event during the ButtonPress animation
    public void TriggerCurrentButton()
    {
        if (currentButton != null)
        {
            currentButton.OnPressed();
        }
    }
}