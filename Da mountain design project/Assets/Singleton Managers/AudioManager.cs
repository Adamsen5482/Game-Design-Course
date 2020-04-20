using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region AudioManager instance
    private static AudioManager instance;
    public static AudioManager audioManager
    {
        get
        {
            return instance;
        }

        set
        {
            if (audioManager != null && audioManager != value)
            {
                Destroy(value.gameObject);
                return;
            }
            instance = value;
        }
    }
    #endregion

    #region AudioSources
    [HideInInspector] public AudioSource musicSource;
    [HideInInspector] public AudioSource musicSource2;
    [HideInInspector] public AudioSource sfxSource;
    [HideInInspector] public AudioSource sfxSource2;
    #endregion

    #region Audio Clip Objects
    public enum CharacterNum {GrassWalk = 0, GrassRun = 1, GravelWalk = 2, GravelRun = 3, WaterWalk = 4, WaterRun = 5, BodySplash = 6};
    public enum EnvironmentNum {WaterWaves = 0, WindBlows = 1};
    public enum StateNum {Winning = 0, LeaderboardClick = 1, Click = 2, Confirm = 3, Cancel = 4, HookClick = 5};
    [Header("Sound Effects")]
    public SfxObject[] characterObjects;
    public SfxObject[] environmentObjects;
    public SfxObject[] stateObjects;
    [Header("Background Music")]
    public MusicObject[] musicObjects;
    public MusicObject[] uiMusicObjects;
    private List<MusicObject> availableMusicObjects = new List<MusicObject>();
    private List<MusicObject> availableUIMusicObjects = new List<MusicObject>();
    [HideInInspector] public CharacterNum characterNum;
    [HideInInspector] public EnvironmentNum environmentNum;
    [HideInInspector] public StateNum stateNum;
    #endregion

    [HideInInspector] public bool activeMusicSource;


    private void Awake()
    {
        audioManager = this;
        DontDestroyOnLoad(gameObject);
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource2 = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < musicObjects.Length; i++)
        {
            AddMusicObject(musicObjects[i]); 
        }

        for (int i = 0; i < uiMusicObjects.Length; i++)
        {
            AddUIMusicObject(uiMusicObjects[i]);
        }

        activeMusicSource = true;

    }

    public void PlayMusic(AudioClip musicClip)
    {
        // Determine which sourcer is active
        AudioSource activeSource = (activeMusicSource) ? musicSource : musicSource2;
        musicSource.clip = musicClip;
        activeSource.volume = 1;
        musicSource.Play();
    }

    public void PlayMusicObject(MusicObject musicObject)
    {
        // Determine which sourcer is active
        AudioSource activeSource = (activeMusicSource) ? musicSource : musicSource2;
        activeSource.clip = musicObject.audioClip;
        activeSource.volume = musicObject.volume;
        activeSource.pitch = musicObject.pitch;
        activeSource.Play();
    }

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        // Determine which source is active
        AudioSource activeSource = (activeMusicSource) ? musicSource : musicSource2;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        // Determine which source is active
        AudioSource activeSource = (activeMusicSource) ? musicSource : musicSource2;
        AudioSource newSource = (activeMusicSource) ? musicSource2 : musicSource;

        // Swap the source 
        activeMusicSource = !activeMusicSource;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    public void PlayMusicWithCrossFade(MusicObject musicObject, float transitionTime = 1.0f)
    {
        // Determine which source is active
        AudioSource activeSource = (activeMusicSource) ? musicSource : musicSource2;
        AudioSource newSource = (activeMusicSource) ? musicSource2 : musicSource;

        // Swap the source 
        activeMusicSource = !activeMusicSource;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = musicObject.audioClip;
        newSource.pitch = musicObject.pitch;
        //newSource.volume = musicObject.volume;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    public void SceneLoadCrossFade(MusicObject musicObject, float transitionTime = 1.0f)
    {

        // Determine which source is active
        AudioSource activeSource = (activeMusicSource) ? musicSource : musicSource2;
        AudioSource newSource = (activeMusicSource) ? musicSource2 : musicSource;

        // Swap the source
        activeMusicSource = !activeMusicSource;
        newSource.clip = musicObject.audioClip;
        newSource.pitch = musicObject.pitch;
        newSource.volume = musicObject.volume;
        newSource.Play();
        //print(newSource.isPlaying);
        StartCoroutine(SceneLoadCrossFade(activeSource, newSource, transitionTime, musicObject.volume));

    }

    public void PauseMusic()
    {
        AudioSource activeSource = musicSource.isPlaying ? musicSource : musicSource2;

        activeSource.Pause();
    }

    public void UnPause()
    {
        AudioSource activeSource = musicSource.isPlaying ? musicSource : musicSource2;

        activeSource.Pause();
    }

    public void PauseMusic(float waitTime)
    {
        AudioSource activeSource = musicSource.isPlaying ? musicSource : musicSource2;

        activeSource.Pause();
        StartCoroutine(PauseMusic(activeSource, waitTime));

    }

    #region SFX Methods
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFXObject(SfxObject sfxObject, float volume = 1.0f)
    {
        sfxSource.pitch = sfxObject.pitch;
        sfxSource.volume = sfxObject.volume;
        sfxObject.volumeScale *= sfxObject.volume * volume;
        sfxSource.PlayOneShot(sfxObject.audioClip, sfxObject.volumeScale);
    }

    public void PlaySFXObject(SfxObject sfxObject)
    {
        sfxSource.pitch = sfxObject.pitch;
        sfxSource.volume = sfxObject.volume;
        sfxObject.volumeScale *= sfxObject.volume;
        sfxSource.PlayOneShot(sfxObject.audioClip, sfxObject.volumeScale);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFXExtra(AudioClip clip)
    {
        sfxSource2.PlayOneShot(clip);
    }

    public void PlaySFXAsMusic(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlaySFXAsMusic(AudioClip clip, float stopTime)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
        StartCoroutine(StopMusic(sfxSource, stopTime));
    }

    public void StopSFX(AudioSource sfxSource)
    {
        sfxSource.Stop();
    }

    #endregion

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void SetPitch(float pitch, AudioSource audioSource)
    {
        audioSource.pitch = pitch;
    }

    public MusicObject RandomMusicObject()
    {
        if (availableMusicObjects != null)
        {
            int random = Random.Range(0, availableMusicObjects.Count);
            MusicObject musicObject = availableMusicObjects[random];
            availableMusicObjects.RemoveAt(random);
            return musicObject;
        } else
        {
            return null;
        }
    }

    public MusicObject RandomUIMusicObject()
    {
        if (availableMusicObjects != null)
        {
            int random = Random.Range(0, availableUIMusicObjects.Count);
            MusicObject musicObject = availableUIMusicObjects[random];
            availableUIMusicObjects.RemoveAt(random);
            return musicObject;
        }
        else
        {
            return null;
        }
    }

    public void AddMusicObject(MusicObject musicObject)
    {
        availableMusicObjects.Add(musicObject);
    }

    public void AddMusicObject(MusicObject musicObject, bool inGame)
    {
        //availableMusicObjects.Add(musicObject);

        if (inGame)
        {
            availableMusicObjects.Add(musicObject);
        }
        else
        {
            availableUIMusicObjects.Add(musicObject);
        }
    }

    public void AddUIMusicObject(MusicObject musicObject)
    {
        availableUIMusicObjects.Add(musicObject);
    }

    #region Private Coroutine Methods

    // Method: fades out the current audio clip and fades in a new. 
    private IEnumerator UpdateMusicWithFade(AudioSource acitveSource, AudioClip newClip, float transitionTime)
    {
        // Make sure the source is active and playing 
        if (!acitveSource.isPlaying)
        {
            acitveSource.Play();
        }

        // Fade Out
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            acitveSource.volume = (1 - (t / Time.deltaTime)); // Lower the volume of the active source for each second.
            yield return null;
        }

        // Stop the current clip, assign the current clip to the new clip and play it. 
        acitveSource.Stop();
        acitveSource.clip = newClip;
        acitveSource.Play();

        // Fade In
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            acitveSource.volume = (t / Time.deltaTime); // increase the volume of the active source for each second.
            yield return null;
        }
    }

    // Fades out current source and fades in new source simultaneously.
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {

        for (float t = 0.0f; t <= transitionTime; t += Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime)); // Fades out the current source by lowering the volume.
            newSource.volume = (t / transitionTime); // Fades in the new source by increasing the volume.
            //print(Time.deltaTime);
            yield return null;
        }

        original.Stop();

    }

    private IEnumerator SceneLoadCrossFade(AudioSource original, AudioSource newSource, float transitionTime, float newSourceVolume)
    {
        for (float t = 0.0f; t <= transitionTime; t += Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime)); // Fades out the current source by lowering the volume.
            newSource.volume = (t / transitionTime); // Fades in the new source by increasing the volume.
            yield return null;
        }
        /*
        if (!newSource.isPlaying)
        {
            newSource.Play();   
        }
        */
        original.Stop();
    }

    private IEnumerator PauseMusic(AudioSource activeSource, float waitTime = 1.0f)
    {
        yield return new WaitForSeconds(waitTime);
        activeSource.UnPause();
    }

    private IEnumerator StopMusic(AudioSource activeSource, float waitTime = 60.0f)
    {
        yield return new WaitForSeconds(waitTime);
        activeSource.Stop();
    }
    #endregion
}
