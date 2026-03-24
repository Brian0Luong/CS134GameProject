using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CharacterController controller = other.GetComponent<CharacterController>();
        PlayerController player = other.GetComponent<PlayerController>();

        if (controller != null)
            controller.enabled = false;

        other.transform.position = spawnPoint.position;
        other.transform.rotation = spawnPoint.rotation;

        if (player != null)
            player.ResetMovementState();

        if (controller != null)
            controller.enabled = true;
    }
}