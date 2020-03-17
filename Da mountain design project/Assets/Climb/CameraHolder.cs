using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace climb
{
    public class CameraHolder : MonoBehaviour
    {
        public Transform target;
        public float speed = 9f;
        public static CameraHolder instance;

        private void Awake()
        {
            instance = this;
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 p = Vector3.Lerp(transform.position, target.position, Time.deltaTime*speed);
            transform.position = p;
        }
    }
}