using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class win : MonoBehaviour
{
    public GameObject winCanvas;
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
            string min = ((int)timer.currentTime / 60).ToString();
            string sec = (timer.currentTime % 60).ToString("f0");
            winTime.text = "YOUR TIME: \n" + min + " minutes and " + sec + " seconds";
            winCanvas.SetActive(true);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
