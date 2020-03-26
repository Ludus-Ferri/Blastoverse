using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTextController : MonoBehaviour
{
    public string splashStringPrefix;

    private void Awake()
    {
        LocalizedText textComponent = GetComponent<LocalizedText>();
        textComponent.SetText(splashStringPrefix);
    }
}
