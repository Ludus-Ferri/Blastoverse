using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour
{
    [Header("Value Manip")]
    public float value;
    float lerpedValue;
    public float lerpSmoothing;

    [Header("UI Elements")]
    public Image lerpedImage;
    public Image actualImage;

    public Color baseColor, updateColor;
    private Color color;
    public float colorSmoothing;

    private void Awake()
    {
        lerpedValue = value;
    }

    // Update is called once per frame
    void Update()
    {
        lerpedValue = Mathf.Lerp(lerpedValue, value, lerpSmoothing * Time.deltaTime);
        UpdateUI();
    }

    void UpdateUI()
    {
        lerpedImage.fillAmount = lerpedValue;
        actualImage.fillAmount = value;

        color = Color.Lerp(color, baseColor, colorSmoothing * Time.deltaTime);

        actualImage.color = color;
    }

    public void Flash()
    {
        color = updateColor;
    }

}
