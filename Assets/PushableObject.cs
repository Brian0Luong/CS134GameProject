using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        LockMovement();
    }

    public void LockMovement()
    {
        rb.constraints =
            RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotation;
    }

    public void UnlockMovement()
    {
        rb.constraints =
            RigidbodyConstraints.FreezeRotation;
    }
}