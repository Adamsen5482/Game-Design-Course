using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterAttack : MonoBehaviour
{
    //public float speed = 1; 
     public Vector3 speed = new Vector3(0,0.2f,0);
    public UnityEngine.GameObject canvas;
    // Start is called before the first frame update
    AudioSource audioSource;
    float winColDistance;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WaterWaves].audioClip;
        audioSource.volume = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WaterWaves].volume;
        audioSource.pitch = AudioManager.audioManager.environmentObjects[(int)AudioManager.EnvironmentNum.WaterWaves].pitch;
        audioSource.PlayOneShot(audioSource.clip);
        winColDistance = Vector3.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Win").GetComponent<Transform>().position);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 7){
        transform.position += speed * Time.deltaTime;
        }
        else if(transform.position.y < 72 && transform.position.y > 7){
        transform.position += speed * Time.deltaTime * 2;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "Player"){

            //print("meh" + other);
            canvas.SetActive(true);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            audioSource.Stop(); 

        }
    }
}
