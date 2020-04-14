using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GameManager Ínstance
    public static GameManager gameManager
    {

        get
        {
            return instance;
        }

        private set
        {
            if (instance != null)
            {
                Destroy(value.gameObject);
                return;
            }

            instance = value;

        }
    }
    private static GameManager instance = null; // Singleton Instance
    #endregion

    [SerializeField] private Transform[] spawnPoints = null;
    #region Audio Properties
    private float transmissionTime;
    private float t = 0.0f; // Keeps track of time
    private MusicObject musicObject1, musicObject2;
    #endregion

    private void Awake()
    {
        gameManager = this;
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].GetComponent<ItemController>().itemID = i;
        }
    }

    private void Start()
    {
        musicObject1 = AudioManager.audioManager.RandomMusicObject();
        AudioManager.audioManager.PlayMusicObject(musicObject1);
        transmissionTime = musicObject1.transitionTime;
    }

    private void Update()
    {
        
        // Can be done when having multiple background music clips. 
        if (Time.time - t >= AudioManager.audioManager.musicSource.clip.length && AudioManager.audioManager.activeMusicSource)
        {
            musicObject2 = AudioManager.audioManager.RandomMusicObject();
            AudioManager.audioManager.PlayMusicWithCrossFade(musicObject2, transmissionTime);
            transmissionTime = musicObject2.transitionTime;
            AudioManager.audioManager.AddMusicObject(musicObject1);
            t = Time.time;
            AudioManager.audioManager.activeMusicSource = false;
        }
        else if (AudioManager.audioManager.musicSource2.clip != null)
        {
            if (Time.time - t >= AudioManager.audioManager.musicSource2.clip.length - transmissionTime)
            {
                musicObject1 = AudioManager.audioManager.RandomMusicObject();
                AudioManager.audioManager.PlayMusicWithCrossFade(musicObject1, transmissionTime);
                transmissionTime = musicObject1.transitionTime;
                AudioManager.audioManager.AddMusicObject(musicObject2);
                t = Time.time;
                AudioManager.audioManager.activeMusicSource = true;
            }
            
        }
        
        
    }
}
