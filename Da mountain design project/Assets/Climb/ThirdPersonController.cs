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
        //Vector3 camYForward;

        //Transform camHolder;

        Rigidbody rigid;
        Collider col;
        Animator anim;

        FreeClimb fc;
        public float moveSpeed = 4;
        public float rotateSpeed = 9;
        public float jumpSpeed = 15;

        bool OnGround;
        bool KeepOfGround;
        float savedTime;

        bool isClimbing;
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            col = GetComponent<Collider>();

            //camHolder = CameraHolder.instance.transform;
            anim = GetComponentInChildren<Animator>();

            fc = GetComponent < FreeClimb>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isClimbing)
            {
                return;
            }
            OnGround = onGround();
            movement();


        }

        void movement()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //camYForward = camHolder.forward;
            Vector3 v = vertical * transform.forward;
            Vector3 h = horizontal * transform.right;

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

            Vector3 direction = transform.forward * (moveSpeed * moveAmount);
            direction.y = rigid.velocity.y;
            rigid.velocity = direction;

        }

        private void Update()
        {
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
                isClimbing =  fc.checkForClimb();
                if (isClimbing)
                {
                    DisableController();
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
                    Vector3 v = rigid.velocity;
                    v.y = jumpSpeed;
                    rigid.velocity = v;
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
            //DebugLine.instance.SetLine(origin, origin+dir,0);
            if (Physics.Raycast(origin, dir, out hit, 0.41f))
            {
                Debug.Log("ost2");
                return true;
            }
                
            return true;
        }

        public void DisableController()
        {
            rigid.isKinematic = true;
            col.enabled = false;
        }

        public void EnableController()
        {
            rigid.isKinematic = false;
            col.enabled = true;
        }
    }

}