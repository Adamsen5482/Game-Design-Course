using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioTrigger : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WindBlows].audioClip;
        audioSource.volume = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WindBlows].volume;
        audioSource.pitch = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WindBlows].pitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        audioSource.Stop();
    }

}
