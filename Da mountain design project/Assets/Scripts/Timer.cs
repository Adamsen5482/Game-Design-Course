using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public Text timer;
    public float currentTime;
    float startTime;
    public bool notWon = true;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (notWon)
        {


            currentTime = Time.time-startTime;


            string min = ((int)currentTime / 60).ToString();

            float seconds =currentTime % 60;
            string sec;
            if (seconds < 9.5)
            {
                sec = "0" + seconds.ToString("f0");
            }
            else
            {
                sec = seconds.ToString("f0");
            }
             




            timer.text = min + ":" + sec;
        }
        else {
            string min = ((int)currentTime / 60).ToString();
            string sec = (currentTime % 60).ToString("f0");
            timer.text = min+ ":" +sec;
            PlayerPrefs.SetFloat("time", currentTime);
        }
    }
}
