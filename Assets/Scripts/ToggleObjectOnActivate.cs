using UnityEngine;

public class ToggleObjectOnActivate : MonoBehaviour, IButtonActivated
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool destroyObject = false;

    [Header("Sound")]
    [SerializeField] private AudioClip activateSound;
    [SerializeField] private float volume = 1f;

    public void Activate()
    {
        if (targetObject == null)
            return;

        if (activateSound != null)
        {
            AudioSource.PlayClipAtPoint(
                activateSound,
                targetObject.transform.position,
                volume
            );
        }

        if (destroyObject)
        {
            Destroy(targetObject);
        }
        else
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }

    public void SetState(bool state)
    {
        if (targetObject == null)
            return;

        if (activateSound != null)
        {
            AudioSource.PlayClipAtPoint(
                activateSound,
                targetObject.transform.position,
                volume
            );
        }

        targetObject.SetActive(state);
    }
}