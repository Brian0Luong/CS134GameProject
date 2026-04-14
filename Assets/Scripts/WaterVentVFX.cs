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
    [SerializeField] private float bubbleSpeed = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource waterHitSource;
    [SerializeField] private AudioClip waterHitClip;
    [SerializeField] private float lifetimeMultiplier = 0.9f;

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

        if (!isUnderwater && waterY + heightOffset > ventY)
        {
            SetUnderwaterMode();
        }
        else if (isUnderwater && waterY + heightOffset <= ventY)
        {
            SetFlowMode();
        }

        if (isUnderwater && bubbles != null)
        {
            float distanceToSurface = waterY - ventY;

            if (distanceToSurface > 0f)
            {
                float lifetime = (distanceToSurface / bubbleSpeed) * lifetimeMultiplier;

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

        if (waterHitSource != null && waterHitClip != null)
        {
            if (!waterHitSource.isPlaying)
            {
                waterHitSource.clip = waterHitClip;
                waterHitSource.loop = true;
                waterHitSource.Play();
            }
        }
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

        if (waterHitSource != null && waterHitSource.isPlaying)
        {
            waterHitSource.Stop();
        }
    }
}