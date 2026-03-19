using UnityEngine;

public class MovingPlatform : MonoBehaviour, IButtonActivated
{
    [Header("Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float pauseTime = 1f;
    [SerializeField] private bool startActive = false;

    private Transform currentTarget;
    private float pauseTimer;
    private bool isPaused;
    private bool isActive;

    private void Start()
    {
        transform.position = pointA.position;
        currentTarget = pointB;
        isActive = startActive;
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
                isPaused = false;

            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.001f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            isPaused = true;
            pauseTimer = pauseTime;
        }
    }

    public void Activate()
    {
        isActive = true;
    }
}