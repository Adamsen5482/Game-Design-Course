using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace climb
{

    public class climbdetector : MonoBehaviour
    {


        public FreeClimb fc;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("hej");
        }

        private void OnTriggerExit(Collider collision)
        {
            Debug.Log("Running");
            if (collision.gameObject.tag == "mountains")
            {
                fc.DetectSlope();
            }
        
    }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("Running");
            if(collision.gameObject.tag == "mountains")
            {
                fc.DetectSlope();
            }
        }
    }
}
