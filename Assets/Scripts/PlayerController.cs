using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("References")]
	private CharacterController controller;
	[SerializeField] private Transform camera;
	[SerializeField] private Animator animator;

	[Header("Movement Settings")]
	[SerializeField] private float walkSpeed = 5f;
	[SerializeField] private float sprintSpeed = 10f;
	[SerializeField] private float sprintTransitSpeed = 5f;
	[SerializeField] private float turningSpeed = 2f;
	[SerializeField] private float gravity = 9.81f;
	[SerializeField] private float jumpHeight = 2f;


	private float verticalVelocity;
	private float speed;

	[Header("Animations")]
	private int animMoveSpeed;
	private int animJump;
	private int animGrounded;

	[Header("Input")]
	private float moveInput;
	private float turnInput;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		SetupAnimator();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		InputManagement();
		Movement();

	}

	private void Movement()
	{
		GroundMovement();
		Turn();
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

    animator.SetFloat(animMoveSpeed, speed * Mathf.Max(Mathf.Abs(moveInput), Mathf.Abs(turnInput)));
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
		if (controller.isGrounded)
		{
			verticalVelocity = -1;

			animator.SetBool(animGrounded, true);

			if (Input.GetButtonDown("Jump"))
			{
				verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);

				animator.SetTrigger(animJump);
			}
		}
		else
		{
			verticalVelocity -= gravity * Time.deltaTime;

			animator.SetBool(animGrounded, false);
		}
		return verticalVelocity;
	}

	private void SetupAnimator()
	{
		animMoveSpeed = Animator.StringToHash("MoveSpeed");
		animJump = Animator.StringToHash("Jump");
		animGrounded = Animator.StringToHash("Grounded");
	}

	private void InputManagement()
	{
		moveInput = Input.GetAxisRaw("Vertical");
		turnInput = Input.GetAxisRaw("Horizontal");
	}
}
