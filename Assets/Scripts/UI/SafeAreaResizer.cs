using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaResizer : MonoBehaviour
{
    RectTransform panel;

    // Start is called before the first frame update
    private void Awake()
    {
        panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        string os = SystemInfo.operatingSystem;

        if (!os.StartsWith("Android"))
        {
            Debug.LogWarning("Not applying safe area - OS is not Android");
            return;
        }

        int apiLevel = int.Parse(os.Substring(os.IndexOf('-') + 1));

        if (apiLevel < 28)  // Android version older than 9 (Pie)
        {
            Debug.LogWarning("Not applying safe area - Android older than Pie");
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position, anchorMax = safeArea.position + safeArea.size;

        anchorMin /= Screen.width;
        anchorMax /= Screen.height;

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
    }
}
