using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class startButtons : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        AudioManager.audioManager.PlaySFXObject(AudioManager.audioManager.stateObjects[(int)AudioManager.StateNum.Click]);
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void Play()
    {
        AudioManager.audioManager.PlaySFXObject(AudioManager.audioManager.stateObjects[(int)AudioManager.StateNum.Click]);
        //GameManager.gameManager.SwitchMusic(GameManager.gameManager.inGame = true);
        SceneManager.LoadScene("MortenTest"); // which scene is correct?
    }


    public void Restart()
    {
        AudioManager.audioManager.PlaySFXObject(AudioManager.audioManager.stateObjects[(int)AudioManager.StateNum.Click]);
        //GameManager.gameManager.SwitchMusic(GameManager.gameManager.inGame = true);
        SceneManager.LoadScene("MortenTest"); // which scene is correct?
    }
}
