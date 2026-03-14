using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("This Room")]
    public WaterRiser water;
    public DoorOpen exitDoor;
    public RoomZone roomZone;

    [Header("Linked Rooms")]
    public RoomController previousRoom;

    private bool started = false;

    public void EnterRoom()
    {
        if (!started)
        {
            started = true;

            if (water != null)
                water.StartRising();

            if (previousRoom != null)
                previousRoom.ShutDownRoom();
        }
    }

    public void ShutDownRoom()
    {
        if (exitDoor != null)
            exitDoor.SetOpen(false);

        if (water != null)
            water.gameObject.SetActive(false);

        if (roomZone != null)
            roomZone.gameObject.SetActive(false);
    }
}
