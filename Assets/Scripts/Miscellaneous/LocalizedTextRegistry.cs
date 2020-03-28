using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalizedTextRegistry
{
    public static List<LocalizedText> texts;

    public static void Clear()
    {
        texts = new List<LocalizedText>();
    }

    public static void Register(LocalizedText text)
    {
        texts.Add(text);
    }

    public static void UpdateAll()
    {
        foreach (LocalizedText text in texts)
        {
            if (text.gameObject.activeInHierarchy)
                text.SetText(text.textID);
        }
    }
}
