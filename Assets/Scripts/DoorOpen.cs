using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3f, 0);
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool shouldOpen = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        if (shouldOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }
    }

    public void SetOpen(bool open)
    {
        shouldOpen = open;
    }
}