using UnityEngine;

public class WaterVentVFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaterRiser water;
    [SerializeField] private ParticleSystem[] waterFlows;
    [SerializeField] private ParticleSystem bubbles;

    [Header("Settings")]
    [SerializeField] private float heightOffset = 0f;

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