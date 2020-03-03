using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNG : MonoBehaviour
{
    public string seed;
    public bool useSystemTime;

    public System.Random random;

    public void Init()
    {
        if (useSystemTime)
            random = new System.Random();
        else
            SetSeed(seed);
        Debug.Log($"Initialized {GetType().Name} with {(useSystemTime ? "random " : "")}seed {(!useSystemTime ? seed : "")} ");
    }

    public void SetSeed(string seed)
    {
        random = new System.Random(seed.GetHashCode());
    }

    public int Next()
    {
        return random.Next();
    }

    public int Next(int maxValue)
    {
        return random.Next(maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public double NextDouble()
    {
        return random.NextDouble();
    }

    public float NextFloat()
    {
        return (float)NextDouble();
    }

    public float NextFloat(float minValue, float maxValue)
    {
        return (float)NextDouble() * (maxValue - minValue) + minValue;
    }
}
