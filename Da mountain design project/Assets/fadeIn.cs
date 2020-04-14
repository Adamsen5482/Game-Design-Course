using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeIn : MonoBehaviour
{
    private float t;
    public GameObject winCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0;
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
