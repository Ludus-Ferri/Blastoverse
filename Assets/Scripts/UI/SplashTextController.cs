using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTextController : MonoBehaviour
{
    public string splashStringPrefix;
    public int splashCount;

    private void Awake()
    {
        LocalizedText textComponent = GetComponent<LocalizedText>();

        string text = splashStringPrefix + $".{Mathf.RoundToInt(Random.Range(1, splashCount))}";

        textComponent.SetText(text);
    }
}
