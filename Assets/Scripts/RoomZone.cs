using UnityEngine;

public class RoomZone : MonoBehaviour
{
    public WaterRiser water;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerWaterState player = other.GetComponent<PlayerWaterState>();
        if (player != null)
        {
            player.SetCurrentWater(water);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerWaterState player = other.GetComponent<PlayerWaterState>();
        if (player != null && player.currentWater == water)
        {
            player.SetCurrentWater(null);
        }
    }
}
