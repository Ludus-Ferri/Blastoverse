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
    public static List<string> availableCultures;

    private static TextAsset[] textAssets;

    public static void Init()
    {
        dictionary = new Dictionary<CultureInfo, Dictionary<string, string>>();

        GetCultures();
    }

    public static void GetCultures()
    {
        availableCultures = new List<string>();

        textAssets = Resources.LoadAll("Localization", typeof(TextAsset)).Cast<TextAsset>().ToArray();

        foreach (TextAsset asset in textAssets)
        {
            availableCultures.Add(asset.name);
            dictionary.Add(CultureInfo.GetCultureInfo(asset.name), new Dictionary<string, string>());
        }
    }

    public static void ParseTranslations()
    {
        try
        {
            foreach (TextAsset asset in textAssets)
            {
                string[] lines = asset.text.Split('\n');

                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) continue;

                    string key = line.Substring(0, line.IndexOf(' '));
                    string value = line.Substring(line.IndexOf(' ') + 1);

                    dictionary[CultureInfo.GetCultureInfo(asset.name)].Add(key, value);
                }
            }
            Debug.Log("Loaded localization files successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Something went wrong while loading localization files: {e.Message}");
        }
    }

    public static void SetCulture(string culture)
    {
        if (availableCultures.Contains(culture))
            currentCulture = CultureInfo.GetCultureInfo(culture);
        else
        {
            Debug.LogWarning($"Localization for {culture} is not available, defaulting to en-US");
            currentCulture = CultureInfo.GetCultureInfo("en-US");
        }
    }

    public static string GetLocalizedString(string baseString)
    {
        if (!dictionary[currentCulture].ContainsKey(baseString))
            return baseString;
        return dictionary[currentCulture][baseString];
    }
}
