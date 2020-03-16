using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float rotateSpeed;
    public Transform player;
    public Transform pivot;
    public Transform camTarget;
    public bool useOffsetValues;
    public Vector3 offset;



    // Use this for initialization
    void Start()
    {
        if (!useOffsetValues){
            offset = player.position - transform.position;
        }

        pivot.transform.position = player.transform.position;
        pivot.transform.parent = player.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        player.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        pivot.Rotate(-vertical, 0, 0);



        float desiredYAngle = player.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = player.position - (rotation * offset);
    
        transform.LookAt(camTarget);

    }
}