using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class AudioPlayer
{
    public bool playAudio = true;
    public bool allowOverlapping = true;
    public float volume = 1.0f;
    public AudioSource source;
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();

    public virtual void Play()
    {
        if (playAudio && source != null && sounds.Count > 0 && (allowOverlapping || !source.isPlaying))
        {
            source.PlayOneShot(sounds[UnityEngine.Random.Range(0, sounds.Count)], volume);
        }
    }
}
