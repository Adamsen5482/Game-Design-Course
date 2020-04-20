using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region GameManager Instance
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
    private float transitionTime;
    [HideInInspector] public float t = 0.0f; // Keeps track of time
    private MusicObject musicObject1, musicObject2;
    //private MusicObject uiMusicObject1, uiMusicObject2;
    [HideInInspector] public bool inGame = false;
    private bool firstMusic = true;
    #endregion

    private void Awake()
    {
        gameManager = this;
        DontDestroyOnLoad(gameObject);
        /*
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].GetComponent<ItemController>().itemID = i;

        }
        */
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (inGame)
        {
            SwitchMusic(inGame);
            
        }
        
    }

    private void Start()
    {
        if (firstMusic)
        {
            musicObject1 = AudioManager.audioManager.RandomUIMusicObject();
            AudioManager.audioManager.PlayMusicObject(musicObject1);
            transitionTime = musicObject1.transitionTime;
            firstMusic = false;
        }
        
    }
    
    private void Update()
    {
        
        // Can be done when having multiple background music clips. 
        if (Time.time - t >= AudioManager.audioManager.musicSource.clip.length)
        {
            SwitchMusic(inGame);
        }
        else if (AudioManager.audioManager.musicSource2.clip != null)
        {
            
            if (Time.time - t >= AudioManager.audioManager.musicSource2.clip.length - transitionTime)
            {
                print("Runs");
                SwitchMusic(inGame);                
            }
            
        }

    }
    
    public void SwitchMusic(bool inGame)
    {
        //print("Runs");

        if (AudioManager.audioManager.activeMusicSource)
        {
            musicObject2 = inGame ? AudioManager.audioManager.RandomMusicObject() : AudioManager.audioManager.RandomUIMusicObject();
            AudioManager.audioManager.SceneLoadCrossFade(musicObject2, transitionTime);
            transitionTime = musicObject2.transitionTime;
            AudioManager.audioManager.AddMusicObject(musicObject1, inGame);
            AudioManager.audioManager.activeMusicSource = false;
        }
        else
        {
            musicObject1 = inGame ? AudioManager.audioManager.RandomMusicObject() : AudioManager.audioManager.RandomUIMusicObject();
            AudioManager.audioManager.SceneLoadCrossFade(musicObject1, transitionTime);
            transitionTime = musicObject1.transitionTime;
            AudioManager.audioManager.AddMusicObject(musicObject2, inGame);
            AudioManager.audioManager.activeMusicSource = true;
        }

        t = Time.time;
    }

    public void SetInGame()
    {
        inGame = true;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
