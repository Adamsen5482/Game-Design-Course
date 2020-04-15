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
            Quaternion rotation = new Quaternion(0, 0, 0, 1);
            rotation.x += Input.GetAxis("Mouse Y") * 5;
            rotation.x = Mathf.Clamp(rotation.x, -60, 60);

            rotation.y += Input.GetAxis("Mouse X") * 5;
            rotation.y = Mathf.Clamp(rotation.y, -60, 60);

            transform.RotateAround(target.position, Vector3.up, rotation.x);
           transform.RotateAround(target.position, Vector3.forward, rotation.y);
        }
    }
}
