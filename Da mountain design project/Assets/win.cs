using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class win : MonoBehaviour
{
    public UnityEngine.GameObject winCanvas;
    public Timer timer;
    public Text winTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag == "Player"){
            timer.notWon = false;
            float currentTime = PlayerPrefs.GetFloat("time");
            string min = ((int)currentTime / 60).ToString();
            string sec = (currentTime % 60).ToString("f0");
            winTime.text = "YOUR TIME: \n" + min + " minutes and " + sec + " seconds";
            //winCanvas.SetActive(true);
            //new line
            SceneManager.LoadScene("WinCinematic");
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
