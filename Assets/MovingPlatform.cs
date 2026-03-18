using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pauseTime = 1f;
    [SerializeField] private float arriveDistance = 0.05f;

    private Transform currentTarget;
    private float pauseTimer;
    private bool isPaused;

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("MovingPlatform: Assign PointA and PointB.");
            enabled = false;
            return;
        }

        transform.position = pointA.position;
        currentTarget = pointB;
    }

    private void Update()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                currentTarget = currentTarget == pointA ? pointB : pointA;
            }
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget.position) <= arriveDistance)
        {
            transform.position = currentTarget.position;
            isPaused = true;
            pauseTimer = pauseTime;
        }
    }
}