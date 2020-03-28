using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Music")]
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Graphics")]
    public Toggle bloomToggle;
    public Toggle aberrationToggle, fisheyeToggle, grainToggle;
    public Slider maxFPSSlider;
    public TMP_Text fpsText;
    public Button applyButton;

    [Header("Language")]
    public TMP_Dropdown langDropdown;

    [Header("Other")]
    public Button resetHighScoreButton;
    public Button viewCreditsButton, invertControlsButton;
    public TMP_Text invertControlsText;

    private void Awake()
    {
        musicVolSlider.value = Options.musicVolume;
        sfxVolSlider.value = Options.sfxVolume;

        bloomToggle.isOn = Options.postProcessingOptions.bloom;
        aberrationToggle.isOn = Options.postProcessingOptions.aberration;
        fisheyeToggle.isOn = Options.postProcessingOptions.fisheye;
        grainToggle.isOn = Options.postProcessingOptions.grain;

        maxFPSSlider.value = Options.maxFPS / 30;
        fpsText.text = Options.maxFPS.ToString();

        BuildLanguageOptions();

        invertControlsText.text = LocalizedStringManager.GetLocalizedString(Options.invertControls ? "ui.options.other.invertControls.enabled" : "ui.options.other.invertControls.disabled");
    }

    void BuildLanguageOptions()
    {
        langDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int selectedLang = 0;

        for (int i = 0; i < LocalizedStringManager.availableCultures.Count; i++) 
        {
            // TODO: Add country flag image

            string culture = LocalizedStringManager.availableCultures[i];

            if (LocalizedStringManager.availableCultures[i] == Options.currentCulture.Name)
                selectedLang = i;

            string nativeLanguageName = CultureInfo.GetCultureInfo(culture).NativeName;
            nativeLanguageName = nativeLanguageName.Substring(0, nativeLanguageName.IndexOf('(') - 1);
            options.Add(new TMP_Dropdown.OptionData()
            {
                text = nativeLanguageName
            });
        }
        options.Sort(delegate(TMP_Dropdown.OptionData data1, TMP_Dropdown.OptionData data2)
        {
            return data1.text.CompareTo(data2.text);
        });

        langDropdown.AddOptions(options);

        langDropdown.value = selectedLang;
    }

    public void OnMusicVolumeChanged(float value)
    {
        Options.musicVolume = value;
        Options.ApplyAudio();
    }

    public void OnSfxVolumeChanged(float value)
    {
        Options.sfxVolume = value;
        Options.ApplyAudio();
    }

    public void OnEnableBloomChanged(bool value)
    {
        Options.postProcessingOptions.bloom = value;
    }

    public void OnEnableAberrationChanged(bool value)
    {
        Options.postProcessingOptions.aberration = value;
    }

    public void OnEnableFisheyeChanged(bool value)
    {
        Options.postProcessingOptions.fisheye = value;
    }

    public void OnEnableGrainChanged(bool value)
    {
        Options.postProcessingOptions.grain = value;
    }

    public void OnApplyButtonPressed()
    {
        Options.ApplyGraphics();
    }

    public void OnMaxFPSChanged(float value)
    {
        Options.maxFPS = (int)value * 30;

        fpsText.text = Options.maxFPS.ToString();
    }

    public void OnLangDropdownChanged(int choice)
    {
        Options.currentCulture = CultureInfo.GetCultureInfo(LocalizedStringManager.availableCultures[choice]);

        Options.SaveLanguage();
        Options.ApplyLanguage();
        LocalizedTextRegistry.UpdateAll();

    }

    public void OnResetHighscorePressed()
    {
        Debug.Log("Reset highscore modal!");
    }

    public void OnViewCreditsPressed()
    {
        Debug.Log("View credits!");
    }

    public void OnInvertControlsPressed()
    {
        Options.invertControls ^= true;
    }
}
