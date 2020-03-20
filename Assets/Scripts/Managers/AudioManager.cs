using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Channels")]
    public AudioMixerGroup master;
    public AudioMixerGroup music, sfx, ui;

    public GameObject soundObjectPrefab;

    Dictionary<SoundType, AudioMixerGroup> audioChannels;

    Dictionary<string, Sound> sounds;

    private void Awake()
    {
        #region Singleton
        if (Instance == null) Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        sounds = new Dictionary<string, Sound>();
        audioChannels = new Dictionary<SoundType, AudioMixerGroup>
        {
            { SoundType.Generic, master },
            { SoundType.Music, music },
            { SoundType.SFX, sfx },
            { SoundType.UI, ui }
        };
    }

    public void PlaySoundAtPosition(Sound sound, Vector3 position)
    {
        SoundObject obj = Instantiate(soundObjectPrefab, position, Quaternion.identity).GetComponent<SoundObject>();
        obj.transform.parent = transform;
        obj.sound = sound;
        obj.Setup();

        obj.source.outputAudioMixerGroup = audioChannels[sound.type];
        obj.source.Play();
    }

    public Sound GetSound(string path)
    {
        if (!sounds.ContainsKey(path))
            sounds.Add(path, Resources.Load<Sound>($"Sounds/{path}"));

        return sounds[path];
    }
}

