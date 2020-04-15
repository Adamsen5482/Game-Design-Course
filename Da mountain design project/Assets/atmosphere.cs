using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atmosphere : MonoBehaviour
{

    // Start is called before the first frame update

    public float eexposureStart = 1.22f;
    public float exposureMin = 0.2f;
    public float exposureMinusAmount = 0.05f;
     float currentExposure;

    public Transform player;
    Vector3 playerstartPos;
    Vector3 differenceInPos;
    float latestdifference;

    private void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", eexposureStart);
        currentExposure = eexposureStart;
        playerstartPos = player.transform.position;



       // fadeDown();
    }

    private void Update()
    {
        float curDif = difference();
        if (curDif > 3 + latestdifference && currentExposure > exposureMin)
        {
            latestdifference = curDif;
           
            currentExposure += exposureMinusAmount;
        }


        RenderSettings.skybox.SetFloat("_Exposure", currentExposure);
       
    }

    float difference()
    {
        float ypos = Mathf.Abs(playerstartPos.y -player.position.y);
        Debug.Log(ypos);
        return ypos;
    }

  
}
