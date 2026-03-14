using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public DoorOpen door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            door.SetOpen(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            door.SetOpen(false);
        }
    }
}