using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour
{
    public List<CameraEffect> effects;
    Dictionary<Type, CameraEffect> effectDict;

    Camera cam;

    public T GetEffect<T>() where T : CameraEffect
    {
        return (T)effectDict[typeof(T)];
    }

    private void Awake()
    {
        effects = GetComponents<CameraEffect>().ToList();
        cam = GetComponent<Camera>();

        effectDict = new Dictionary<Type, CameraEffect>();
        foreach (CameraEffect effect in effects)
            effectDict.Add(effect.GetType(), effect);

        StartCoroutine(EvaluateEffects());
    }

    IEnumerator EvaluateEffects()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame();

            foreach (CameraEffect effect in effects)
                effect.Perform();

            Vector3 totalDisplacement = Vector3.zero;
            Quaternion totalRotation = Quaternion.identity;
            float totalZoom = 0;

            foreach (CameraEffect effect in effects)
            {
                totalDisplacement += effect.localDisplacement;
                totalRotation *= effect.localRotation;
                totalZoom += effect.zoom;
            }

            transform.localPosition = totalDisplacement;
            transform.localRotation = totalRotation;

        }

    }

    private void LateUpdate()
    {
        
    }
}
