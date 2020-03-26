using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string textID;
    public bool setOnAwake;
    public bool withVariants;

    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        if (setOnAwake) SetText(textID);
    }

    public void SetText(string id)
    {
        textID = id;

        text.text = withVariants ? LocalizedStringManager.GetLocalizedStringRandomVariant(textID) : LocalizedStringManager.GetLocalizedString(textID);
    }
}
