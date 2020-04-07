using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct ProgressionStep
{
    public long scoreThreshold;

    [Header("Multipliers")]
    public float scorePerSecondMult;
    public float asteroidSpawnRateMult;
    public float asteroidVelocityMult;
    public float asteroidSizeMult;

    [Header("Other")]
    public Color backgroundColor;
    public float musicIntensity;
}
