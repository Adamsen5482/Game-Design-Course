using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudioTrigger : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject player;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WindBlows].audioClip;
        audioSource.volume = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WindBlows].volume;
        audioSource.pitch = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WindBlows].pitch;
    }
    private void Update(){
        if (player.transform.position.y > 70){
            if (!audioSource.isPlaying)
            {
            audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

}
