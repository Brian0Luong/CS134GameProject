using UnityEngine;

public class ToggleObjectOnActivate : MonoBehaviour, IButtonActivated
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool setActiveState = true;
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
            targetObject.SetActive(setActiveState);
        }
    }
}