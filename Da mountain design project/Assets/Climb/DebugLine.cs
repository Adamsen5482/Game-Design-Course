using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace climb
{


    public class DebugLine : MonoBehaviour
    {
        public int maxRenderes;

        List<LineRenderer> lines = new List<LineRenderer>();

        void start()
        {
       
        }

        public static DebugLine instance;

        private void Awake()
        {
            instance = this;
        }

        void CreateLine(int i)
        {
            UnityEngine.GameObject go = new UnityEngine.GameObject();
                lines.Add(go.AddComponent<LineRenderer>());
                lines[i].widthMultiplier = 0.05f;
        }

        public void SetLine(Vector3 startpos, Vector3 endpos, int index)
        {
            if (index > lines.Count - 1)
            {
                CreateLine(index);
            }

            lines[index].SetPosition(0, startpos);
            lines[index].SetPosition(1, endpos);
           
        }
    }
}
