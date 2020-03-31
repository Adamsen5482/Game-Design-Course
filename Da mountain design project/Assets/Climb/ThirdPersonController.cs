using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            fc = GetComponent <FreeClimb>();
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

        public void aim()
        {
            //return;
            Aiming = !Aiming;
            if (!useOffsetValues)
            {
                offset = player.position - transform.position;
            }

            pivot.transform.position = player.transform.position;
            pivot.transform.parent = player.transform;

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Aim"))
            {
                aimToggle = !aimToggle;

                if (aimToggle)
                {
                    aim();    
                    print("Works!");
                }
                
            }

            if (Aiming)
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
                return;
            }


            if (isClimbing)
            {
                fc.Tick(Time.deltaTime);
                return;
            }
            OnGround = onGround();

            if (KeepOfGround)
            {
                if (Time.realtimeSinceStartup - savedTime > 0.5f)
                {
                    KeepOfGround = false;
                }
            }



            jump();

            if (!OnGround && !KeepOfGround)
            {
                if (!climfOff)
                {
                    
                    Debug.Log(isClimbing);


                    isClimbing = fc.checkForClimb();
                    if (isClimbing)
                    {
                        DisableController();
                    }
                }
            }

            if (climfOff)
            {
                if (Time.realtimeSinceStartup - climbTimer > 1)
                {
                    climfOff = false;
                }
            }
            anim.SetFloat("move", moveAmount);

            anim.SetBool("OnAir", !OnGround);

        }

        void jump()
        {
            if (OnGround)
            {
                bool jump = Input.GetButtonUp("Jump");

                if (jump)
                {
                    savedTime = Time.realtimeSinceStartup;
                    KeepOfGround = true;
                    
                }
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
            */
            col.enabled = false;
            
            playerMovement.enabled = false;
            Debug.Log("disable controller");
           
        }

        public void EnableController()
        {
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