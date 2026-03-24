using UnityEngine;

public class ToggleObjectOnActivate : MonoBehaviour, IButtonActivated
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool setActiveOnActivate = true;
    [SerializeField] private bool destroyObject = false;

    public void Activate()
    {
        if (targetObject == null)
            return;

        if (destroyObject)
        {
            Destroy(targetObject);
        }
        else
        {
            targetObject.SetActive(setActiveOnActivate);
        }
    }

    public void SetState(bool state)
    {
        if (targetObject == null)
            return;

        targetObject.SetActive(state);
    }
}