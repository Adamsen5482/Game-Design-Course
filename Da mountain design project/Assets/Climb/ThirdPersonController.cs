using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace climb
{


    public class ThirdPersonController : MonoBehaviour
    {
        float horizontal;
        float vertical;
        Vector3 moveDir;
        float moveAmount;
        Vector3 camYForward;

        Transform camHolder;
        
        //Rigidbody rigid;
        CharacterController characterController;
        PlayerMovement playerMovement;
        Collider col;
        Animator anim;

        public FreeClimb fc;
        public float moveSpeed = 4;
        public float rotateSpeed = 9;
        public float jumpSpeed = 15;

        bool climfOff;
        float climbTimer;
        bool OnGround;
        bool KeepOfGround;
        float savedTime;

        public bool isClimbing;

        public bool Aiming = false;
        private bool aimToggle = false;

        public Transform player;
        public Transform pivot;
        public Transform camTarget;
        public bool useOffsetValues;
        public Vector3 offset;


        void Start()
        {
            /*
            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            */
            characterController = GetComponent<CharacterController>();
            playerMovement = GetComponent<PlayerMovement>();
            col = GetComponent<Collider>();

            //camHolder = CameraHolder.instance.transform;
            anim = GetComponentInChildren<Animator>();
            fc = GetComponent<FreeClimb>();
            anim.transform.GetChild(0).GetComponent<Animator>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isClimbing)
            {
                return;
            }
            OnGround = onGround();
            //movement();
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)));


        }

        void movement()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            camYForward = camHolder.forward;
            Vector3 v = vertical * camHolder.forward;
            Vector3 h = horizontal * camHolder.right;

            moveDir = (v + h).normalized;
            moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)));

            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }
            Quaternion lookdirection = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookdirection, Time.deltaTime * rotateSpeed);
            transform.rotation = targetRot;
            /*
            Vector3 direction = transform.forward * (moveSpeed * moveAmount);
            direction.y = rigid.velocity.y;
            rigid.velocity = direction;
            */

        }



        private void Update()
        {
            climb();
            OnGround = onGround();

            if (KeepOfGround)
            {
                if (Time.realtimeSinceStartup - savedTime > 0.5f)
                {
                    KeepOfGround = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {

                fc.climbing = !fc.climbing;
                if (playerMovement.enabled == true)
                {
                    DisableController();
                    anim.SetBool("climbing", true);
                }
                else
                {
                    EnableController();
                    anim.SetBool("climbing", false);
                }

            }
            anim.SetFloat("Moving", moveAmount);


            anim.SetFloat("move", moveAmount);

            anim.SetBool("OnAir", !OnGround);

        }


        void climb()
        {
            if (!fc.climbing)
                return;

            if (fc.NearWall())
            {
                //                Debug.Log("near wall true");
                if (fc.FacingWall() && fc.DetectSlope())
                {
                    fc.ClimbWall(transform);
                }
                else
                {
                    fc.climbing = false;
                    EnableController();
                    anim.SetBool("climbing", false);
                }

            }
            else
            {
                fc.climbing = false;
                EnableController();
                anim.SetBool("climbing", false);
            }

        }

        bool onGround()
        {

            if (KeepOfGround)
            {
                return false;
            }
            Vector3 origin = transform.position;
            origin.y -= 0.4f;
            Vector3 dir = -transform.up;
            RaycastHit hit;

            if (Physics.Raycast(origin, dir, out hit, 0.41f))
            {

                return true;
            }

            return true;
        }

        public void DisableController()
        {
            /*
            rigid.isKinematic = true;
            
            col.enabled = false;
            */
            playerMovement.enabled = false;
            //Debug.Log("disable controller");
            helper.instance.RemoveMessage("Press E to climb");
            StartCoroutine(helper.instance.GetMessage("Press E to walk"));
            
        }

        public void EnableController()
        {
            helper.instance.RemoveMessage("Press E to walk");
            StartCoroutine(helper.instance.GetMessage("Press E to climb"));
            playerMovement.enabled = true;
            //rigid.isKinematic = false;
            col.enabled = true;

            anim.CrossFade("blend", 0.2f);
            anim.SetBool("OnAir", true);

            climfOff = true;
            climbTimer = Time.realtimeSinceStartup;
            isClimbing = false;

        }
    }

}