using UnityEngine;

public class CubeSlideSound : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource slideSource;
    public AudioSource impactSource;

    [Header("Sliding Sound")]
    public float moveThreshold = 0.05f;
    public float fallThreshold = -0.1f;

    [Header("Landing Sound")]
    public AudioClip landClip;
    public float minImpactSpeed = 1f;
    public float landVolumeMultiplier = 0.2f;

    private Rigidbody rb;
    private bool forceSlidingSound = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (slideSource != null)
        {
            slideSource.loop = true;
            slideSource.playOnAwake = false;
        }

        if (impactSource != null)
        {
            impactSource.loop = false;
            impactSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (rb == null || slideSource == null)
            return;

        Vector3 velocity = rb.velocity;
        bool isFalling = velocity.y < fallThreshold;

        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0f;

        bool isMovingHorizontally = horizontalVelocity.magnitude > moveThreshold;
        bool shouldPlaySlide = (isMovingHorizontally || forceSlidingSound) && !isFalling;

        if (shouldPlaySlide)
        {
            if (!slideSource.isPlaying)
                slideSource.Play();
        }
        else
        {
            if (slideSource.isPlaying)
                slideSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactSource == null || landClip == null)
            return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                float impactSpeed = collision.relativeVelocity.magnitude;

                if (impactSpeed >= minImpactSpeed)
                {
                    float volume = Mathf.Clamp(impactSpeed * landVolumeMultiplier, 0.2f, 1f);
                    impactSource.PlayOneShot(landClip, volume);
                }

                break;
            }
        }
    }

    public void SetSlidingSoundActive(bool active)
    {
        forceSlidingSound = active;
    }
}