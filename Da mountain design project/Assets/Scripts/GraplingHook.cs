using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraplingHook : MonoBehaviour
{
    public Transform player;
    public UnityEngine.GameObject GraplingHookVisual;
    public Image Crosshair;
    public int timeToReachTarget;
    // Start is called before the first frame update
    void Start()
    {

    Crosshair.GetComponent<Image>().color = new Color32(255,0,0,100);
    }

    // Update is called once per frame
    void Update()
    {
        Crosshair.transform.position = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast (ray, out hitInfo) && hitInfo.transform.tag == "Hookable"){

            Debug.DrawLine (ray.origin, hitInfo.point, Color.red);
            Crosshair.GetComponent<Image>().color = new Color32(0,255,0,100);

        } else{
            Debug.DrawLine (ray.origin, ray.origin+ray.direction*100 , Color.green);
            Crosshair.GetComponent<Image>().color = new Color32(255,0,0,100);
        }

        if (Input.GetMouseButton(0) && Physics.Raycast (ray, out hitInfo) && hitInfo.transform.tag == "Hookable"){
            Debug.Log("click");
            GraplingHookVisual.SetActive(true);
            StartCoroutine(lerpPosition(player.transform.position, hitInfo.transform.position, timeToReachTarget));
        }
    }

     IEnumerator lerpPosition( Vector3 StartPos, Vector3 EndPos, float LerpTime)
    {
        player.GetComponent<CharacterController>().enabled = false;
        float StartTime = Time.time;
        float EndTime = StartTime + LerpTime;
 
        while(Time.time < EndTime)
        {
            float timeProgressed = (Time.time - StartTime) / LerpTime;  // this will be 0 at the beginning and 1 at the end.
            player.transform.position = Vector3.Lerp(StartPos, EndPos, timeProgressed);
 
            yield return new WaitForFixedUpdate();
        }
        GraplingHookVisual.SetActive(false);
        player.GetComponent<CharacterController>().enabled = true;
 
    }
    
}

