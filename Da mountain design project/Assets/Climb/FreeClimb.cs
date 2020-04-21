using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace climb
{
    public class FreeClimb : MonoBehaviour
    {
        public float ClimbSpeed = 3f;
        public LayerMask wallMask;
        public CharacterController controller;
        public bool climbing;

        Vector3 wallPoint;
        Vector3 wallNormal;
        //public Rigidbody body;
        // public CapsuleCollider coll;

        public bool NearWall()
        {
            return Physics.CheckSphere(transform.position, 3f, wallMask);

        }

        public bool FacingWall()
        {
            RaycastHit hit;
            var facingWall = Physics.Raycast(transform.position, transform.forward, out hit, controller.radius + 5f, wallMask);
            wallPoint = hit.point;
            wallNormal = hit.normal;
            return facingWall;
        }

        public void ClimbWall(Transform target)
        {
            //body.useGravity = false;

            GrabWall();

            var v = Input.GetAxis("Vertical");
            var h = Input.GetAxis("Horizontal");

            var move = transform.up * v + transform.right * h;
            transform.position += transform.right * h * Time.deltaTime * 2f;

            transform.position += transform.up * v * Time.deltaTime * 2f;
            // ApplyMove(move, ClimbSpeed);
        }

        public void GrabWall()
        {
            var newPosition = wallPoint + wallNormal * (controller.radius - 0.1f);
            Vector3 cheese = new Vector3(1, 1, 1);

            if (Vector3.Distance(newPosition, transform.position) > 1)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.deltaTime);
            }


            if (wallNormal == Vector3.zero)
                return;

            // transform.rotation = Quaternion.LookRotation(-wallNormal);
        }

        public bool DetectSlope()
        {
            return true;
        }



    }
}
