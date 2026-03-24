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
            DeactivateTargets();
        }
    }

    void ActivateTargets()
    {
        foreach (MonoBehaviour target in activationTargets)
        {
            if (target is IButtonActivated activatable)
            {
                activatable.Activate();
            }
        }
    }

    void DeactivateTargets()
    {
        foreach (MonoBehaviour target in activationTargets)
        {
            if (target is IButtonActivated activatable)
            {
                activatable.Activate(); // toggle behavior
            }
        }
    }
}