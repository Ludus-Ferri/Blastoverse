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
    Dictionary<string, CameraEffect> effectDict;

    Camera cam;

    public CameraEffect GetEffect(string name)
    {
        return effectDict[name];
    }

    private void Awake()
    {
        effects = GetComponents<CameraEffect>().ToList();
        cam = GetComponent<Camera>();

        effectDict = new Dictionary<string, CameraEffect>();
        foreach (CameraEffect effect in effects)
            effectDict.Add(effect.GetType().Name, effect);

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
