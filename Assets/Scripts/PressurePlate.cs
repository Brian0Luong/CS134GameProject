using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private MonoBehaviour[] activationTargets;

    [Header("Settings")]
    [SerializeField] private bool requirePushable = true;

    private int objectsOnPlate = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (requirePushable && !other.CompareTag("Pushable"))
            return;

        objectsOnPlate++;

        if (objectsOnPlate == 1)
        {
            ActivateTargets();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (requirePushable && !other.CompareTag("Pushable"))
            return;

        objectsOnPlate--;

        if (objectsOnPlate <= 0)
        {
            objectsOnPlate = 0;
            DeactivateTargets();
        }
    }

    private void ActivateTargets()
    {
        foreach (MonoBehaviour target in activationTargets)
        {
            if (target is ToggleObjectOnActivate toggleTarget)
            {
                toggleTarget.SetState(true);
            }
            else if (target is DoorOpen door)
            {
                door.SetOpen(true);
            }
            else if (target is IButtonActivated activatable)
            {
                activatable.Activate();
            }
        }
    }

    private void DeactivateTargets()
    {
        foreach (MonoBehaviour target in activationTargets)
        {
            if (target is ToggleObjectOnActivate toggleTarget)
            {
                toggleTarget.SetState(false);
            }
            else if (target is DoorOpen door)
            {
                door.SetOpen(false);
            }
        }
    }
}