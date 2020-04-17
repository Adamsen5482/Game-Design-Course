using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideMouse : MonoBehaviour
{
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject leaderboardCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(winCanvas.active || loseCanvas.active || leaderboardCanvas.active){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        print("mouseTrue");
        }
        else{
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
