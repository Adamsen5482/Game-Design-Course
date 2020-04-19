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
            var facingWall = Physics.Raycast(transform.position, transform.forward, out hit, controller.radius + .9f, wallMask);
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
            // Raycast with infinite distance to check the slope directly under the player no matter where they are
            RaycastHit hit;
            Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity);

            // Saving the normal
            Vector3 n = hit.normal;

            // Crossing my normal with the player's up vector (if your player rotates I guess you can just use Vector3.up to create a vector parallel to the ground
            Vector3 groundParallel = Vector3.Cross(transform.up, n);

            // Crossing the vector we made before with the initial normal gives us a vector that is parallel to the slope and always pointing down
            Vector3 slopeParallel = Vector3.Cross(groundParallel, n);
            Debug.DrawRay(hit.point, slopeParallel * 10, Color.green);

            // Just the current angle we're standing on
            float currentSlope = Mathf.Round(Vector3.Angle(hit.normal, transform.up));
            //Debug.Log(currentSlope);

            // If the slope is on a slope too steep and the player is Grounded the player is pushed down the slope.
            if (currentSlope >= 110f )
            {
                return false;
               
            }
            return true;

        }



    }
}
