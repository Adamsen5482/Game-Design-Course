﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class startButtons : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityEngine.GameObject leaderboardCanvas;
    public UnityEngine.GameObject winCanvas;
    public UnityEngine.GameObject loseCanvas;
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
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void Play()
    {
        
        SceneManager.LoadScene("MortenTest"); // which scene is correct?
    }


    public void Restart()
    {

        SceneManager.LoadScene("MortenTest"); // which scene is correct?
    }

    public void Leaderboard()
    {
        leaderboardCanvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }
}
