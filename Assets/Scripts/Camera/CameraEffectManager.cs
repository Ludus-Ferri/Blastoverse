using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour
{
    public List<CameraEffect> effects;

    private void Awake()
    {
        effects = GetComponents<CameraEffect>().ToList();
    }

    private void LateUpdate()
    {
        Vector3 totalDisplacement = Vector3.zero;
        Quaternion totalRotation = Quaternion.identity;

        foreach (CameraEffect effect in effects)
        {
            totalDisplacement += effect.localDisplacement;
            totalRotation *= effect.localRotation;
        }

        transform.localPosition = totalDisplacement;
        transform.localRotation = totalRotation;
    }
}
