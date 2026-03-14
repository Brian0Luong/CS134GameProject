using UnityEngine;

public class RoomStartTrigger : MonoBehaviour
{
    public RoomController roomController;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggered) return;

        triggered = true;
        roomController.EnterRoom();
    }
}
