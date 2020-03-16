using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using UnityEngine;

public class LocalizedStringManager
{
    public static Dictionary<CultureInfo, Dictionary<string, string>> dictionary;

    public static CultureInfo currentCulture;
    public static string availableCultures;

    private static TextAsset[] textAssets;

    public static void Init()
    {
        dictionary = new Dictionary<CultureInfo, Dictionary<string, string>>();

        GetCultures();
    }

    public static void GetCultures()
    {
        textAssets = Resources.LoadAll("Localization", typeof(TextAsset)).Cast<TextAsset>().ToArray();

        foreach (TextAsset asset in textAssets)
        {
            dictionary.Add(CultureInfo.GetCultureInfo(asset.name), new Dictionary<string, string>());
        }
    }

    public static void ParseTranslations()
    {
        foreach (TextAsset asset in textAssets)
        {
            string[] lines = asset.text.Split('\n');

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) continue;

                dictionary[CultureInfo.GetCultureInfo(asset.name)].Add(line.Substring(0, line.IndexOf(' ')), line.Substring(line.IndexOf(' ') + 1));
            }
            
        }
        Debug.Log("Loaded localization files successfully!");
    }

    public static void SetCulture(string culture)
    {
        currentCulture = CultureInfo.GetCultureInfo(culture);
    }

    public static string GetLocalizedString(string baseString)
    {
        if (!dictionary[currentCulture].ContainsKey(baseString))
            return baseString;
        return dictionary[currentCulture][baseString];
    }
}
