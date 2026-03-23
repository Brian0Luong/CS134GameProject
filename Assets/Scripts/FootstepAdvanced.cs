using UnityEngine;

public class FootstepAdvanced : MonoBehaviour
{
    public AudioSource audioSource;
    public float stepDelay = 0.5f;

    private float stepTimer;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(audioSource.clip);
                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}