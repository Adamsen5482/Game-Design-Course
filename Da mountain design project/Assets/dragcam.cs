using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragcam : MonoBehaviour
{
    public Transform target;
    public GameObject othercam;
    int degrees =2;
    // Update is called once per frame

    private void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.rotation = othercam.transform.rotation;
            transform.position = othercam.transform.position;
        }

        if (Input.GetMouseButton(1))
        {
          
           
            transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * degrees);
           transform.RotateAround(target.position, Vector3.forward, Input.GetAxis("Mouse Y") * degrees);
        }
    }
}
