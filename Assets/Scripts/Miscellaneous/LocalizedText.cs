using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string textID;

    private void Awake()
    {
        GetComponent<TMP_Text>().text = LocalizedStringManager.GetLocalizedString(textID);
    }
}
