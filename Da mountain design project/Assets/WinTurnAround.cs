using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTurnAround : MonoBehaviour
{
    public GameObject winCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles.y<350){
            if(transform.eulerAngles.y<250){

                transform.Rotate(0.0f, 0.5f, 0.0f, Space.Self);
            } else{
                
                winCanvas.SetActive(true);
                transform.Rotate(0.0f, 0.5f, 0.0f, Space.Self);
        }
        }
    }
}
