using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using UnityEngine;

public class Localizer
{
    public static CultureInfo GetCurrentCultureInfo()
    {
        SystemLanguage currentLanguage = Application.systemLanguage;
        CultureInfo correspondingCultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.EnglishName.Equals(currentLanguage.ToString()));
        return CultureInfo.CreateSpecificCulture(correspondingCultureInfo.TwoLetterISOLanguageName);
    }
}
