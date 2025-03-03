using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraExplosionShake : MonoBehaviour
{
    public float shakeIntensity = 2f;
    public float shakeSpeed = 10f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2f - 1f;
        float y = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2f - 1f;

        transform.localPosition = originalPosition + new Vector3(x, y, 0) * shakeIntensity;
    }
}

