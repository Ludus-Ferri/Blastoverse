using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MathHelper
{
    public static float Logerp(float a, float b, float t)
    {
        return a * Mathf.Pow(b / a, t);
    }
}