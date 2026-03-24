using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    [SerializeField] private Transform camera;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform pushCheckPoint;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float sprintTransitSpeed = 5f;
    [SerializeField] private float turningSpeed = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpCooldown = 1f;

    [Header("Push Settings")]
    [SerializeField] private float pushCheckRadius = 0.35f;
    [SerializeField] private float pushCheckDistance = 0.5f;
    [SerializeField] private float pushMoveSpeed = 2.5f;
    [SerializeField] private float playerDistanceFromPushable = 0.9f;
    [SerializeField] private float pushAlignSpeed = 12f;
    [SerializeField] private float pushRotationAlignSpeed = 12f;
    [SerializeField] private LayerMask pushableLayer;

    private float verticalVelocity;
    private float speed;
    private float jumpTimer;

    [Header("Animations")]
    private int animMoveSpeed;
    private int animJump;
    private int animGrounded;
    private int animIsPushing;
    private int animPushSpeed;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private bool isPushing = false;
    private bool movementLocked;
    private Rigidbody currentPushable;
    private Vector3 pushNormal;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        SetupAnimator();
        ResetMovementState();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        InputManagement();
        jumpTimer -= Time.deltaTime;

        if (isPushing)
        {
            PushMovement();
        }
        else
        {
            Movement();
            TryEnterPushMode();
        }
    }

    private void Movement()
    {
        if (movementLocked) return;
        GroundMovement();
        Turn();
    }

    public void LockMovement()
    {
        movementLocked = true;
    }

    public void UnlockMovement()
    {
        movementLocked = false;
    }

    private void GroundMovement()
    {
        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * moveInput + camRight * turnInput).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, sprintSpeed, sprintTransitSpeed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, sprintTransitSpeed * Time.deltaTime);
        }

        Vector3 velocity = move * speed;
        velocity.y = VerticalForceCalculation();

        controller.Move(velocity * Time.deltaTime);

        float moveAmount = Mathf.Max(Mathf.Abs(moveInput), Mathf.Abs(turnInput));
        animator.SetFloat(animMoveSpeed, speed * moveAmount);
        animator.SetBool(animIsPushing, false);
        animator.SetFloat(animPushSpeed, 0f);
    }

    private void Turn()
    {
        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 lookDirection = (camForward * moveInput + camRight * turnInput).normalized;

        if (lookDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed);
        }
    }

    private float VerticalForceCalculation()
    {
        bool grounded = controller.isGrounded;
        bool previouslyGrounded = animator.GetBool(animGrounded);

        animator.SetBool(animGrounded, grounded);

        if (grounded)
        {
            if (!previouslyGrounded)
            {
                jumpTimer = jumpCooldown;
            }

            verticalVelocity = -2f;

            if (!isPushing && Input.GetButtonDown("Jump") && jumpTimer <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
                animator.SetTrigger(animJump);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        return verticalVelocity;
    }

    private void TryEnterPushMode()
    {
        if (!controller.isGrounded) return;
        if (pushCheckPoint == null) return;

        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput + camRight * turnInput).normalized;

        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Vector3 origin = controller.bounds.center + moveDirection * 0.3f;

        if (Physics.SphereCast(origin, pushCheckRadius, moveDirection, out RaycastHit hit, pushCheckDistance, pushableLayer, QueryTriggerInteraction.Ignore))
        {
            if (!hit.collider.CompareTag("Pushable")) return;

            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb == null) return;
            if (rb.velocity.y < -0.1f) return;

            Collider col = hit.collider;
            Bounds bounds = col.bounds;

            Vector3 localOffset = transform.position - bounds.center;
            localOffset.y = 0f;

            float absX = Mathf.Abs(localOffset.x);
            float absZ = Mathf.Abs(localOffset.z);

            float cornerTolerance = 0.15f;
            if (Mathf.Abs(absX - absZ) < cornerTolerance)
            {
                return;
            }

            currentPushable = rb;
            isPushing = true;

            PushableObject pushable = currentPushable.GetComponent<PushableObject>();
            if (pushable != null)
            {
                pushable.UnlockMovement();
            }

            if (absX > absZ)
            {
                pushNormal = new Vector3(Mathf.Sign(localOffset.x), 0f, 0f);
            }
            else
            {
                pushNormal = new Vector3(0f, 0f, Mathf.Sign(localOffset.z));
            }

            animator.SetBool(animIsPushing, true);
            animator.SetFloat(animPushSpeed, 0f);
        }
    }

    private void PushMovement()
    {
        if (currentPushable == null)
        {
            ExitPushMode();
            return;
        }

        if (currentPushable.velocity.y < -0.1f)
        {
            ExitPushMode();
            return;
        }

        if (!IsCurrentPushableStillInRange())
        {
            ExitPushMode();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ExitPushMode();
            return;
        }

        AlignToPushable();

        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput + camRight * turnInput).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Vector3 move = moveDirection * pushMoveSpeed * Time.deltaTime;

            currentPushable.MovePosition(currentPushable.position + move);
            controller.Move(move);

            animator.SetFloat(animPushSpeed, 1f);
        }
        else
        {
            animator.SetFloat(animPushSpeed, 0f);
        }

        float verticalMove = VerticalForceCalculation();
        controller.Move(Vector3.up * (verticalMove * Time.deltaTime));

        animator.SetBool(animIsPushing, true);
        animator.SetFloat(animMoveSpeed, 0f);
    }

    private bool IsCurrentPushableStillInRange()
    {
        if (currentPushable == null) return false;

        Collider pushableCollider = currentPushable.GetComponent<Collider>();
        if (pushableCollider == null) return false;

        Vector3 checkDirection = -pushNormal;
        checkDirection.y = 0f;
        checkDirection.Normalize();

        Vector3 origin = transform.position + Vector3.up * 1.0f + checkDirection * 0.3f;
        Vector3 sphereCenter = origin + checkDirection * pushCheckDistance;

        Vector3 closestPoint = pushableCollider.ClosestPoint(sphereCenter);
        float distance = Vector3.Distance(sphereCenter, closestPoint);

        return distance <= pushCheckRadius;
    }

    private void AlignToPushable()
    {
        if (currentPushable == null) return;

        Vector3 boxPos = currentPushable.position;

        Vector3 desiredPos = boxPos + pushNormal * playerDistanceFromPushable;
        desiredPos.y = transform.position.y;

        Vector3 toDesired = desiredPos - transform.position;
        toDesired.y = 0f;

        float distanceToDesired = toDesired.magnitude;

        if (distanceToDesired > 0.05f)
        {
            Vector3 alignMove = toDesired.normalized * Mathf.Min(distanceToDesired, pushAlignSpeed * Time.deltaTime);
            controller.Move(alignMove);
        }

        Vector3 lookDir = boxPos - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                pushRotationAlignSpeed * Time.deltaTime
            );
        }
    }

    private void ExitPushMode()
    {
        if (currentPushable != null)
        {
            PushableObject pushable = currentPushable.GetComponent<PushableObject>();
            if (pushable != null)
            {
                pushable.LockMovement();
            }
        }

        isPushing = false;
        currentPushable = null;

        animator.SetBool(animIsPushing, false);
        animator.SetFloat(animPushSpeed, 0f);
    }

    private void SetupAnimator()
    {
        animMoveSpeed = Animator.StringToHash("MoveSpeed");
        animJump = Animator.StringToHash("Jump");
        animGrounded = Animator.StringToHash("Grounded");
        animIsPushing = Animator.StringToHash("IsPushing");
        animPushSpeed = Animator.StringToHash("PushSpeed");
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
    }

    private void OnDrawGizmosSelected()
    {
        if (camera == null) return;

        Vector3 camForward = camera.forward;
        Vector3 camRight = camera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput + camRight * turnInput).normalized;
        if (moveDirection.sqrMagnitude < 0.01f)
            moveDirection = transform.forward;

        Vector3 origin = transform.position + Vector3.up * 1.0f + moveDirection * 0.3f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, pushCheckRadius);
        Gizmos.DrawLine(origin, origin + moveDirection * pushCheckDistance);
        Gizmos.DrawWireSphere(origin + moveDirection * pushCheckDistance, pushCheckRadius);
    }

    public void ResetMovementState()
    {
        verticalVelocity = -2f;
        speed = 0f;
        jumpTimer = 0f;

        movementLocked = false;
        isPushing = false;
        currentPushable = null;
        pushNormal = Vector3.zero;

        animator.SetBool(animGrounded, false);
        animator.SetBool(animIsPushing, false);
        animator.SetFloat(animPushSpeed, 0f);
        animator.SetFloat(animMoveSpeed, 0f);
    }
}