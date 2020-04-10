using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioObject : ScriptableObject
{
    public AudioClip audioClip;
    // Audio Source Properties
    public float pitch = 1.0f;
    public float volume = 1.0f;

    public virtual void OnEnable()
    {

    }

    public virtual void Play(AudioSource audioSource)
    {

    }

    public virtual void Play()
    {

    }

    public virtual void OnDisable()
    {
        // Subclasses can override this method
    }
}
