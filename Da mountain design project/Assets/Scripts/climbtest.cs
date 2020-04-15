using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class climbtest : MonoBehaviour
{
 public float WalkSpeed = 5f;
    public float ClimbSpeed = 3f;
    public LayerMask wallMask;
    public CharacterController controller;
    bool climbing;

    Vector3 wallPoint;
    Vector3 wallNormal;

   // Rigidbody body;
    CapsuleCollider coll;

    // Use this for initialization
    void Start()
    {
       // body = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        if (NearWall())
        {
            if (FacingWall())
            {
                // if player presses the climb button
                if (Input.GetKeyUp(KeyCode.C))
                {
                    climbing = !climbing;
                }
            }
            else
            {
                climbing = false;
            }
        }
        else
        {
            climbing = false;
        }

        if (climbing)
        {
            ClimbWall();
        }
        else
        {
            Walk();
        }
    }

    void Walk()
    {
        //body.useGravity = true;

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = transform.forward * v + transform.right * h;

        ApplyMove(move, WalkSpeed);
    }

    bool NearWall()
    {
        return Physics.CheckSphere(transform.position, 3f, wallMask);
    }

    bool FacingWall()
    {
        RaycastHit hit;
        var facingWall = Physics.Raycast(transform.position, transform.forward, out hit, coll.radius + 1f, wallMask);
        wallPoint = hit.point;
        wallNormal = hit.normal;
        return facingWall;
    }

    void ClimbWall()
    {
        //body.useGravity = false;

        GrabWall();

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var move = transform.up * v + transform.right * h;

        ApplyMove(move, ClimbSpeed);
    }

    void GrabWall()
    {
        var newPosition = wallPoint + wallNormal * (coll.radius - 0.1f);
        transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.deltaTime);

        if (wallNormal == Vector3.zero)
            return;

        transform.rotation = Quaternion.LookRotation(-wallNormal);
    }

    void ApplyMove(Vector3 move, float speed)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = speed * Input.GetAxis("Vertical");
        float curSpeedY =  speed * Input.GetAxis("Horizontal");

        Vector3 moveDirection = (forward * curSpeedX) + (right * curSpeedY);

 
    moveDirection.y -= 20* Time.deltaTime;
    controller.Move(moveDirection* Time.deltaTime);
    }
}

