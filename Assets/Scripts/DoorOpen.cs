using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorOpen : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3f, 0);

    [Header("Movement Speeds")]
    public float openSpeed = 2f;
    public float closeSpeed = 2f;

    [Header("Sound")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public float volume = 1f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool shouldOpen = false;

    private AudioSource audioSource;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
        float speed = shouldOpen ? openSpeed : closeSpeed;

        if (shouldOpen)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                openPosition,
                Time.deltaTime * speed
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                closedPosition,
                Time.deltaTime * speed
            );
        }
    }

    public void SetOpen(bool open)
    {
        if (open != shouldOpen)
        {
            PlayDoorSound(open);
        }

        shouldOpen = open;
    }

    void PlayDoorSound(bool opening)
    {
        if (audioSource == null) return;

        AudioClip clip = opening ? openSound : closeSound;

        if (clip != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clip, volume);
        }
    }
}