using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string textID;
    public bool setOnAwake;

    private void Awake()
    {
        if (setOnAwake) SetText(textID);
    }

    public void SetText(string id)
    {
        textID = id;
        GetComponent<TMP_Text>().text = LocalizedStringManager.GetLocalizedString(textID);
    }
}
