using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX", menuName = "Audio/SFX")]
public class SfxObject : AudioObject
{
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip != null ? audioClip : null;
        audioSource?.Play();
    }

}
