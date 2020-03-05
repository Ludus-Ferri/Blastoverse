using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : CameraEffect
{
    public float perlinFrequency, perlinRotateFrequency;
    public float perlinAmplitude, perlinRotateAmplitude;

    public float damping;

    public float trauma;
    public float traumaExponent = 2.3f;

    float seed;

    [SerializeField]
    float zPosition = -10;

    public void InduceMotion(float t)
    {
        trauma = Mathf.Clamp01(trauma + t);
    }

    private void Start()
    {
        seed = GenericRNG.Instance.NextFloat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InduceMotion(1);
    }

    private void LateUpdate()
    {
        float shakeStrength = Mathf.Pow(trauma, traumaExponent);

        SetDisplacement(new Vector3(
            (Mathf.PerlinNoise(seed, Time.time * perlinFrequency) * 2 - 1) * perlinAmplitude * shakeStrength,
            (Mathf.PerlinNoise(seed + 1, Time.time * perlinFrequency) * 2 - 1) * perlinAmplitude * shakeStrength, 
            zPosition));

        SetRotation(Quaternion.Euler(0, 0,
            (Mathf.PerlinNoise(seed + 2, Time.time * perlinRotateFrequency) * 2 - 1) * perlinRotateAmplitude * shakeStrength
            ));

        trauma = Mathf.Lerp(trauma, 0, damping * Time.deltaTime);
    }
}
