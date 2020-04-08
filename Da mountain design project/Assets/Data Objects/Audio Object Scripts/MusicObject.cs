using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicObject : AudioObject
{
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip != null ? audioClip : null;
        audioSource?.Play();
    }
}
