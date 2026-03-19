using System.Collections;
using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform buttonVisual;
    [SerializeField] private MonoBehaviour[] activationTargets;

    [Header("Press Settings")]
    [SerializeField] private float pressDistance = 0.02f;
    [SerializeField] private float pressSpeed = 8f;
    [SerializeField] private Vector3 pressDirection = Vector3.back;
    [SerializeField] private bool returnAfterPress = false;
    [SerializeField] private float returnDelay = 0.2f;

    private PlayerInteraction player;
    private Vector3 startLocalPos;
    private bool isPressed;
    private Coroutine pressRoutine;

    private void Start()
    {
        if (buttonVisual != null)
            startLocalPos = buttonVisual.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        if (interaction != null)
        {
            player = interaction;
            player.SetInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        if (interaction != null && interaction == player)
        {
            player.ClearInteractable(this);
            player = null;
        }
    }

    public void OnPressed()
    {
        if (isPressed)
            return;

        isPressed = true;

        if (pressRoutine != null)
            StopCoroutine(pressRoutine);

        pressRoutine = StartCoroutine(PressButtonRoutine());

        ActivateTargets();
    }

    private void ActivateTargets()
    {
        foreach (MonoBehaviour target in activationTargets)
        {
            if (target is IButtonActivated activatable)
            {
                activatable.Activate();
            }
        }
    }

    private IEnumerator PressButtonRoutine()
    {
        if (buttonVisual == null)
            yield break;

        Vector3 targetPos = startLocalPos + pressDirection.normalized * pressDistance;

        while (Vector3.Distance(buttonVisual.localPosition, targetPos) > 0.001f)
        {
            buttonVisual.localPosition = Vector3.Lerp(
                buttonVisual.localPosition,
                targetPos,
                Time.deltaTime * pressSpeed
            );
            yield return null;
        }

        buttonVisual.localPosition = targetPos;

        if (returnAfterPress)
        {
            yield return new WaitForSeconds(returnDelay);

            while (Vector3.Distance(buttonVisual.localPosition, startLocalPos) > 0.001f)
            {
                buttonVisual.localPosition = Vector3.Lerp(
                    buttonVisual.localPosition,
                    startLocalPos,
                    Time.deltaTime * pressSpeed
                );
                yield return null;
            }

            buttonVisual.localPosition = startLocalPos;
            isPressed = false;
        }
    }
}