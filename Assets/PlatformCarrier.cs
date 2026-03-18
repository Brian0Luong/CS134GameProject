using UnityEngine;

public class PlatformCarrier : MonoBehaviour
{
    [SerializeField] private Transform platformRoot;

    private void OnTriggerEnter(Collider other)
    {
        if (platformRoot == null) return;

        if (other.CompareTag("Pushable") || other.CompareTag("Player"))
        {
            other.transform.SetParent(platformRoot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable") || other.CompareTag("Player"))
        {
            if (other.transform.parent == platformRoot)
            {
                other.transform.SetParent(null);
            }
        }
    }
}