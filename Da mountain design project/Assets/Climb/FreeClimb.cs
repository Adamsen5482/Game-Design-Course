using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace climb
{
    public class FreeClimb : MonoBehaviour
    {
        public Animator anim;
        public bool isClimbing;
        bool inPosition;
        public bool isMid;
        bool isLerping;
        float T;
        Vector3 startPos;
        Vector3 targetPos;
        Quaternion startRot;
        Quaternion targetRot;
        public float possOffset = 0.5f;
        public float rayToMoveDir = 0.5f;
        public float wallOffset = 0.3f;
        public float speed = 0.2f;
        public float climbSpeed = 3;
        public float rotateSpeed = 5;
        public float rayForwardsToWall = 1;
        public IKSnapshot baseIKsnapshot;
        Transform helper;
        float delta;
        LayerMask ignoreLayer;
        float horizontal;
        float vertical;

        public FreeClimbHook a_hook;
        public ThirdPersonController tpc;
        void Start()
        {
            tpc = GetComponent<ThirdPersonController>();
            Init();
        }

        public void Init()
        {
            helper = new GameObject().transform;
            helper.name = "climb helper";
            a_hook.init(this, helper);
            ignoreLayer = ~(1 << 8);
            //checkForClimb();
        }
        // Update is called once per frame
    

        public void Tick(float d_time)
        {
            this.delta = d_time;
            if (!inPosition)
            {
                getInPosition(delta);
                return;
            }

            if (!isLerping)
            {
                 horizontal = Input.GetAxis("Horizontal");
                 vertical = Input.GetAxis("Vertical");
                float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

                Vector3 h = helper.right * horizontal;
                Vector3 v = helper.up * vertical;
                Vector3 movedir = (h + v).normalized;

                if (isMid)
                {
                    if (movedir == Vector3.zero)
                    {
                        return;
                    }
                }
                else
                {
                    bool canMove = CanMove(movedir);
                    if (!canMove || movedir == Vector3.zero)
                    {
                        return;
                    }
                }

                isMid = !isMid;

                T = 0;
                isLerping = true;
                startPos = transform.position;
                 Vector3 tp = helper.position - transform.position;
                float d = Vector3.Distance(helper.position, startPos) / 2;
                tp *= possOffset;
                tp += transform.position;
                targetPos = isMid ? tp : helper.position;
               
                a_hook.CreatePosition(targetPos, movedir, isMid);

            }
            else
            {
                T += delta * climbSpeed;
                if (T > 1)
                {
                    T = 1;
                    isLerping = false;
                }

                Vector3 cp = Vector3.Lerp(startPos, targetPos, T);
                transform.position = cp;
                transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
            }
        }
        bool CanMove(Vector3 movedir)
        {
            Vector3 origin = transform.position;
            float dis = rayToMoveDir;
            Vector3 dir = movedir;

            DebugLine.instance.SetLine(origin, origin + (dir * dis), 0);
            //raycast towars the direction you want to move 
            RaycastHit hit;

            if (Physics.Raycast(origin, dir, out hit, dis))
            {
                //check if corner 
                return false;
            }

            origin += movedir * dis;
            dir = helper.forward;
            float dis2 = rayForwardsToWall;
            //raycast forward to the wall
            DebugLine.instance.SetLine(origin, origin + (dir * dis2), 1);
            if (Physics.Raycast(origin, dir, out hit, dis2))
            {
                helper.position = posWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }

            origin = origin + (dir * dis2);
            dir = -movedir;
            DebugLine.instance.SetLine(origin, origin + dir, 1);
            //corner raycast
            if (Physics.Raycast(origin, dir, out hit, rayForwardsToWall))
            {
                helper.position = posWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }


                // return false;
                origin += dir * dis2;
            dir = -Vector3.up;
            DebugLine.instance.SetLine(origin, origin + dir , 2);

            if (Physics.Raycast(origin, dir, out hit, dis2))
            {
                float angle = Vector3.Angle(-helper.forward, hit.normal);
                if (angle < 40)
                {
                    helper.position = posWithOffset(origin, hit.point);
                    helper.rotation = Quaternion.LookRotation(-hit.normal);
                    return true;
                }
            }



            return false;
        }


        public void getInPosition(float delta)
        {
            T += delta*3;

            if (T > 1)
            {
                T = 1;
                inPosition = true;
                horizontal = 0;
                vertical = 0;
                a_hook.CreatePosition(targetPos, Vector3.zero, false);
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, T);
            transform.position = tp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }

        Vector3 posWithOffset(Vector3 origin, Vector3 target)
        {
            Vector3 direction = origin - target;
            direction.Normalize();
            Vector3 offset = direction * wallOffset;
            return target + offset;
        }

        // check that the player is in climbing position with raycasting
        public bool checkForClimb()
        {
            Vector3 Origin = transform.position;
            Origin.y += 0.02f;
            Vector3 dir = transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(Origin, dir, out hit, 0.5f, ignoreLayer))
            {
                helper.position = posWithOffset(Origin, hit.point);
                initForClimb(hit);
                return true;
            }
            return false;
        }

        void initForClimb(RaycastHit hit)
        {
            isClimbing = true;

            helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
            startPos = transform.position;
            targetPos = hit.point + (hit.normal * wallOffset);
            T = 0;
            inPosition = false;
            anim.CrossFade("climb_idle", 2);

        }

        void LookForGround()
        {
            RaycastHit hit;
            Vector3 origin = transform.position;
            Vector3 dir = -Vector3.up;
            if(Physics.Raycast(origin, dir, out hit, 1.2f, ignoreLayer))
            {
                isClimbing = false;
                tpc.EnableController();
            }

        }
    }
    [System.Serializable]
    public class IKSnapshot
        {
            public Vector3 rh, lh, lf, rf;
        }


    
}
