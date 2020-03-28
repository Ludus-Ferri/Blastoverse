using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Options
{
    protected const string op_musicVol = "Options_MusicVol",
        op_sfxVol = "Options_SfxVol",
        op_bloom = "Options_Bloom",
        op_aberration = "Options_Aberration",
        op_fisheye = "Options_Fisheye",
        op_grain = "Options_Grain",
        op_maxFPS = "Options_MaxFPS",
        op_lang = "Options_Lang",
        op_invertControls = "Options_InvertControls";

    public static float musicVolume;
    public static float sfxVolume;

    public static PostProcessingOptions postProcessingOptions;
    public static int maxFPS;

    public static CultureInfo currentCulture;

    public static bool invertControls;

    public static void Save()
    {
        PlayerPrefs.SetFloat(op_musicVol, musicVolume);
        PlayerPrefs.SetFloat(op_sfxVol, sfxVolume);

        PlayerPrefs.SetInt(op_bloom, postProcessingOptions.bloom ? 1 : 0);
        PlayerPrefs.SetInt(op_aberration, postProcessingOptions.aberration ? 1 : 0);
        PlayerPrefs.SetInt(op_fisheye, postProcessingOptions.fisheye ? 1 : 0);
        PlayerPrefs.SetInt(op_grain, postProcessingOptions.grain ? 1 : 0);

        PlayerPrefs.SetInt(op_maxFPS, maxFPS);

        SaveLanguage();

        PlayerPrefs.SetInt(op_invertControls, invertControls ? 1 : 0);

        PlayerPrefs.Save();
    }

    public static void SaveLanguage()
    {
        PlayerPrefs.SetString(op_lang, currentCulture.Name);
    }

    public static void Load()
    {
        musicVolume = PlayerPrefs.GetFloat(op_musicVol, 0f);
        sfxVolume = PlayerPrefs.GetFloat(op_sfxVol, 0f);

        postProcessingOptions.bloom = PlayerPrefs.GetInt(op_bloom, 1) == 1;
        postProcessingOptions.aberration = PlayerPrefs.GetInt(op_aberration, 1) == 1;
        postProcessingOptions.fisheye = PlayerPrefs.GetInt(op_fisheye, 1) == 1;
        postProcessingOptions.grain = PlayerPrefs.GetInt(op_grain, 1) == 1;

        maxFPS = PlayerPrefs.GetInt(op_maxFPS, 60);

        string name = PlayerPrefs.GetString(op_lang, "invalid");
        if (name != "invalid") 
            currentCulture = CultureInfo.GetCultureInfo(name);
        else currentCulture = null;

        invertControls = PlayerPrefs.GetInt(op_invertControls, 0) == 1;
    }

    public static void Apply()
    {
        ApplyLanguage();
        ApplyAudio();
        ApplyGraphics();

        // TODO: Invert controls
    }

    public static void ApplyLanguage()
    {
        LocalizedStringManager.SetCulture(currentCulture.Name);
    }

    public static void ApplyAudio()
    {
        AudioManager.Instance.master.audioMixer.SetFloat("RawMusicVolume", musicVolume);
        AudioManager.Instance.master.audioMixer.SetFloat("RawSFXVolume", sfxVolume);
    }

    public static void ApplyGraphics()
    {
        GameManager.Instance.postProcessingProfile.TryGet(out Bloom bloom);
        bloom.active = postProcessingOptions.bloom;
        GameManager.Instance.postProcessingProfile.TryGet(out ChromaticAberration aberration);
        aberration.active = postProcessingOptions.aberration;
        GameManager.Instance.postProcessingProfile.TryGet(out LensDistortion fisheye);
        fisheye.active = postProcessingOptions.fisheye;
        GameManager.Instance.postProcessingProfile.TryGet(out FilmGrain grain);
        grain.active = postProcessingOptions.grain;

        Application.targetFrameRate = maxFPS;
    }
}
