using UnityEngine;

public class WaterVentVFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaterRiser water;
    [SerializeField] private ParticleSystem[] waterFlows;
    [SerializeField] private ParticleSystem bubbles;

    [Header("Settings")]
    [SerializeField] private float heightOffset = 0f;

    [Header("Bubble Settings")]
    [SerializeField] private float bubbleSpeed = 1.5f; // MUST match particle Start Speed

    private bool isUnderwater = false;

    private void Start()
    {
        SetFlowMode();
    }

    private void Update()
    {
        if (water == null)
            return;

        float waterY = water.CurrentWaterY;
        float ventY = transform.position.y;

        // 🔄 Switch between flow and bubbles
        if (!isUnderwater && waterY + heightOffset > ventY)
        {
            SetUnderwaterMode();
        }
        else if (isUnderwater && waterY + heightOffset <= ventY)
        {
            SetFlowMode();
        }

        // 🫧 OPTION 2: Dynamically adjust bubble lifetime
        if (isUnderwater && bubbles != null)
        {
            float distanceToSurface = waterY - ventY;

            if (distanceToSurface > 0f)
            {
                float lifetime = distanceToSurface / bubbleSpeed;

                var main = bubbles.main;
                main.startLifetime = lifetime;
            }
        }
    }

    private void SetFlowMode()
    {
        isUnderwater = false;

        if (waterFlows != null)
        {
            foreach (ParticleSystem flow in waterFlows)
            {
                if (flow != null)
                    flow.Play();
            }
        }

        if (bubbles != null)
            bubbles.Stop();
    }

    private void SetUnderwaterMode()
    {
        isUnderwater = true;

        if (waterFlows != null)
        {
            foreach (ParticleSystem flow in waterFlows)
            {
                if (flow != null)
                    flow.Stop();
            }
        }

        if (bubbles != null)
            bubbles.Play();
    }
}