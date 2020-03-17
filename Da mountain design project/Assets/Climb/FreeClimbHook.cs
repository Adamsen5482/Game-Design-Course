using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace climb
{


    public class FreeClimbHook : MonoBehaviour
    {
        Animator anim;
        IKSnapshot IKbase;
        IKSnapshot current = new IKSnapshot();
        IKSnapshot next = new IKSnapshot();
        IKGoals goals = new IKGoals();
        public float w_rh;
        public float w_lh;
        public float w_rf;
        public float w_lf;
        public float lerpspeed = 1f;
        float delta;
        Vector3 rh, lh, lf, rf;
        Transform h;
        bool isMirror;
        bool isLeft;
        Vector3 prevMoveDir;

        public void init(FreeClimb c, Transform helper)
        {
            anim = c.anim;
            IKbase = c.baseIKsnapshot;
            h = helper;

        }

        public void CreatePosition(Vector3 origin,Vector3 moveDir, bool isMid)
        {
            delta = Time.deltaTime;
            HandleAnim(moveDir, isMid);

            if (!isMid)
            {
                UpdateGoals(moveDir);
                prevMoveDir = moveDir;
            }
            else
            {
                UpdateGoals(prevMoveDir);
            }

            IKSnapshot ik = CreateSnapShot(origin);
            CopySnapShat(ref current, ik);

            SetIkPosition(isMid, goals.lf, current.lf, AvatarIKGoal.LeftFoot);
           
            SetIkPosition(isMid, goals.rf, current.rf, AvatarIKGoal.RightFoot);
            SetIkPosition(isMid, goals.lh, current.lh, AvatarIKGoal.LeftHand);
            SetIkPosition(isMid, goals.rh, current.rh, AvatarIKGoal.RightHand);


            UpdateIKWeight(AvatarIKGoal.LeftFoot, 1);
            UpdateIKWeight(AvatarIKGoal.RightFoot, 1);
            UpdateIKWeight(AvatarIKGoal.LeftHand, 1);
            UpdateIKWeight(AvatarIKGoal.RightHand, 1);
        }
        
        void UpdateGoals(Vector3 moveDir)
        {
            isLeft = (moveDir.x <= 0);

            if (moveDir.x != 0)
            {
                goals.lh = isLeft;
                goals.lf = isLeft;
                goals.rh = !isLeft;
                goals.rf = !isLeft;
           }
            else
            {
                bool isEnabled = isMirror;
                if (moveDir.y < 0)
                {
                    isEnabled = !isEnabled;
                }
                goals.lh = isEnabled;
                goals.lf = isEnabled;
                goals.rh = !isEnabled;
                goals.rf = !isEnabled;
            }
        }


        void HandleAnim(Vector3 moveDir, bool isMid)
        {
            if (isMid)
            {
                if (moveDir.y != 0)
                {
                    if (moveDir.x == 0)
                    {
                        isMirror = !isMirror;
                        anim.SetBool("mirror", isMirror);

                    }
                    else
                    {



                        if (moveDir.y < 0)
                        {
                            isMirror = (moveDir.x > 0);
                            anim.SetBool("mirror", isMirror);
                        }
                        else
                        {
                            isMirror = (moveDir.x < 0);
                            anim.SetBool("mirror", isMirror);
                        }
                    }
                    anim.CrossFade("climb_up", 0.2f);
                }
             
            }
            else
            {
                anim.CrossFade("climb_idle", 0.2f);
            }
        }

        public IKSnapshot CreateSnapShot(Vector3 origin)
        {
            IKSnapshot r = new IKSnapshot();
           // r.lh = LocalToWorld(IKbase.lh);
          //  r.rh = LocalToWorld(IKbase.rh);
           // r.lf = LocalToWorld(IKbase.lf);
           // r.rf = LocalToWorld(IKbase.rf);

            Vector3 _lh = LocalToWorld(IKbase.lh);
            r.lh = GetPosActual(_lh, AvatarIKGoal.LeftHand);

            Vector3 _lf = LocalToWorld(IKbase.lf);
            r.lf = GetPosActual(_lf, AvatarIKGoal.LeftFoot);

            Vector3 _rh = LocalToWorld(IKbase.rh);
            r.rh = GetPosActual(_rh, AvatarIKGoal.RightHand);

            Vector3 _rf = LocalToWorld(IKbase.rf);
            r.rf = GetPosActual(_rf, AvatarIKGoal.RightFoot);
            return r;
        }
        public float walloffset = 0.0f;
        Vector3 GetPosActual(Vector3 o, AvatarIKGoal goal)
        {
            Vector3 r = o;
            Vector3 origin = o;
            Vector3 dir = h.forward;
            origin += -(dir * 0.2f);
            RaycastHit hit;

            bool isHit =false;
            if(Physics.Raycast(origin, dir, out hit, 1.5f))
            {
                Vector3 _r = hit.point + (hit.normal*walloffset);
                r = _r;
                isHit = true;

                if(goal==AvatarIKGoal.LeftFoot||goal== AvatarIKGoal.RightFoot)
                {
                    if (hit.point.y > transform.position.y - 0.1f)
                    {
                        isHit = false;
                    }
                }
            }

            if (!isHit)
            {
                switch (goal)
                {
                    case AvatarIKGoal.LeftFoot:
                        r = LocalToWorld(IKbase.lf);
                        break;
                    case AvatarIKGoal.RightFoot:
                        r = LocalToWorld(IKbase.rf);
                        break;
                    case AvatarIKGoal.LeftHand:
                        r = LocalToWorld(IKbase.lh);
                        break;
                    case AvatarIKGoal.RightHand:
                        r = LocalToWorld(IKbase.rh);
                        break;
                    default:
                        break;
                }
               
            }
            return r;
        }


        Vector3 LocalToWorld(Vector3 p)
        {
            Vector3 r = h.position;
            r += h.right * p.x;
            r += h.forward * p.z;
            r += h.up * p.y;
            return r;
        }
        public void CopySnapShat(ref IKSnapshot to, IKSnapshot from)
        {
            to.rh = from.rh;
            to.lh = from.lh;
            to.rf = from.rf;
            to.lf = from.lf;
        }

        void SetIkPosition(bool isMid, bool isTrue, Vector3 pos, AvatarIKGoal goal)
        {
            if (isMid)
            {
                Vector3 p = GetPosActual(pos, goal);
                if (isTrue)
                {
                  
                    UpdateIKPosition(goal, p);
                }
                else
                {
                    if (goal == AvatarIKGoal.LeftFoot || goal == AvatarIKGoal.RightFoot)
                    {
                        if (p.y > transform.position.y - 0.05f)
                        {
                          //  UpdateIKPosition(goal, p);
                        }
                    }
                }
             
            }
            else
            {
                if (!isTrue)
                {
                    Vector3 p = GetPosActual(pos, goal);
                    UpdateIKPosition(goal, p);
                }
            }

        }

        public void UpdateIKPosition(AvatarIKGoal goal, Vector3 pos)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    lf = pos;
                    break;
                case AvatarIKGoal.RightFoot:
                    rf = pos;
                    break;
                case AvatarIKGoal.LeftHand:
                    lh = pos;
                    break;
                case AvatarIKGoal.RightHand:
                    rh = pos;
                    break;
                default:
                    break;
            }
        }

        public void UpdateIKWeight(AvatarIKGoal goal, float w)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    w_lf = w;
                    break;
                case AvatarIKGoal.RightFoot:
                    w_rf = w;
                    break;
                case AvatarIKGoal.LeftHand:
                    w_lh = w;
                    break;
                case AvatarIKGoal.RightHand:
                    w_rh = w;
                    break;
                default:
                    break;
            }
        }


        public void OnAnimatorIK()
        {
            delta = Time.deltaTime;
            SetIKPos(AvatarIKGoal.LeftHand, lh, w_lh);
            SetIKPos(AvatarIKGoal.RightHand, rh, w_rh);
            SetIKPos(AvatarIKGoal.LeftFoot, lf, w_lf);
            SetIKPos(AvatarIKGoal.RightFoot, rf, w_rf);
        }

        void SetIKPos(AvatarIKGoal goal, Vector3 targetpos, float w)
        {
            IKStates ikstate = GetIKStates(goal);
            if (ikstate == null)
            {
                ikstate = new IKStates();
                ikstate.goal = goal;
                ikStates.Add(ikstate);

            }

            if (w == 0)
            {
                ikstate.isSet = false;
            }

            if (!ikstate.isSet)
            {
                ikstate.position = GoalToBodyBones(goal).position;
                ikstate.isSet = true;
            }

            ikstate.positionWeight = w;
            ikstate.position = Vector3.Lerp(ikstate.position, targetpos, delta*lerpspeed);
            anim.SetIKPositionWeight(goal, ikstate.positionWeight);
            anim.SetIKPosition(goal, ikstate.position);
        }

        Transform GoalToBodyBones(AvatarIKGoal goal)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    return anim.GetBoneTransform(HumanBodyBones.LeftFoot);
                 
                case AvatarIKGoal.RightFoot:
                    return anim.GetBoneTransform(HumanBodyBones.RightFoot);
                
                case AvatarIKGoal.LeftHand:
                    return anim.GetBoneTransform(HumanBodyBones.LeftHand);
                
                default:
                case AvatarIKGoal.RightHand:
                    return anim.GetBoneTransform(HumanBodyBones.RightHand);
                    break;
            }
        }

        List<IKStates> ikStates = new List<IKStates>();

        IKStates GetIKStates(AvatarIKGoal goal)
        {
            IKStates r = null;
            foreach (IKStates i in ikStates)
            {
                if (i.goal == goal)
                {
                    r = i;
                    break;
                }
            }
            return r;
        }

        class IKStates
        {
            public AvatarIKGoal goal;
            public Vector3 position;
            public float positionWeight;
            public bool isSet = false;
            
        }
    }

    public class IKGoals
    {
        public bool rh;
        public bool lh;
        public bool rf;
        public bool lf;
    }
}