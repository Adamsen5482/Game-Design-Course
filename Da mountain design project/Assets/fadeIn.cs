using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeIn : MonoBehaviour
{
    private float t;
    public GameObject winCanvas;
    public Text wintext;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        float currentTime = PlayerPrefs.GetFloat("time");
        string min = ((int)currentTime / 60).ToString();
        string sec = (currentTime % 60).ToString("f0");
        wintext.text = "YOUR TIME: \n" + min + " minutes and " + sec + " seconds";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (winCanvas.activeSelf){
            if(GetComponent<CanvasGroup>().alpha < 1){
                t += 0.5f * Time.deltaTime;
                GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, t);
            }
        }
    }
}
