using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "Audio/Music")]
public class MusicObject : AudioObject
{
    public float transitionTime = 1.0f;

    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip != null ? audioClip : null;
        audioSource?.Play();
    }

    public void Stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Pause(AudioSource audioSource)
    {
        audioSource.Pause();
    }
}
