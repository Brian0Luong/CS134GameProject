using UnityEngine;

public class LandingSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip landingClip;

    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    private bool wasGrounded;

    void Start()
    {
        wasGrounded = IsGrounded();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        // 
        if (!wasGrounded && isGrounded)
        {
            PlayLandingSound();
        }

        wasGrounded = isGrounded;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

void PlayLandingSound()
{
    if (audioSource == null || landingClip == null) return;

    audioSource.PlayOneShot(landingClip, 1.5f);
}
}