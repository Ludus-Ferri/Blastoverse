using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public Vector3 localDisplacement;
    public Quaternion localRotation;

    internal void SetDisplacement(Vector3 displacement)
    {
        localDisplacement = displacement;
    }

    internal void SetRotation(Quaternion rotation)
    {
        localRotation = rotation;
    }
}
