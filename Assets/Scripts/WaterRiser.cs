using UnityEngine;

public class WaterRiser : MonoBehaviour
{
    public float riseSpeed = 0.4f;
    public float maxHeight = 4f;

    private bool rising = false;

    public float CurrentWaterY => transform.position.y;

    public void StartRising()
    {
        rising = true;
    }

    void Update()
    {
        if (!rising) return;

        Vector3 pos = transform.position;
        pos.y += riseSpeed * Time.deltaTime;
        pos.y = Mathf.Min(pos.y, maxHeight);
        transform.position = pos;
    }
}
