using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public AudioSource source;

    public Sound sound;

    private void OnClipEnd()
    {
        Destroy(gameObject);
    }

    public void Setup()
    {
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.playOnAwake = false;

        gameObject.name = $"Sound ({source.clip.name})";

        Invoke("OnClipEnd", source.clip.length + 1);
    }
}
