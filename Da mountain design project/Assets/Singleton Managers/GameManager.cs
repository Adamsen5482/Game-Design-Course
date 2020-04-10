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
    [SerializeField] private float transmissionTime;
    private float t = 0.0f; // Keeps track of time
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
        print(AudioManager.audioManager.musicObjects[0].volume);
        AudioManager.audioManager.PlayMusicObject(AudioManager.audioManager.musicObjects[0]);
    }

    private void Update()
    {
        
        // Can be done when having multiple background music clips. 
        if (Time.time - t >= AudioManager.audioManager.musicSource.clip.length && AudioManager.audioManager.firstMusicSourceIsPlaying)
        {
            AudioManager.audioManager.PlayMusicWithCrossFade(AudioManager.audioManager.musicObjects[1].audioClip);
            t = Time.time;
        }
        else if (Time.time - t >= AudioManager.audioManager.musicSource2.clip.length - transmissionTime)
        {
            AudioManager.audioManager.PlayMusicWithCrossFade(AudioManager.audioManager.musicObjects[0].audioClip);
            t = Time.time;
        }
        
        
    }
}
