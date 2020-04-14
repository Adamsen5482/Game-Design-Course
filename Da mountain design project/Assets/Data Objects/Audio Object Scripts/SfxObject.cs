using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX", menuName = "Audio/SFX")]
public class SfxObject : AudioObject
{
    public float volumeScale = 1.0f;

    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip != null ? audioClip : null;
        audioSource?.Play();
    }

    public void Stop(AudioSource audioSource)
    {
        audioSource.clip = audioClip != null ? audioClip : null;
        audioSource?.Stop();
    }
}
