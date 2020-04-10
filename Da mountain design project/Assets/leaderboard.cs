using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leaderboard : MonoBehaviour
{
    public Timer timer;
    public string[] names;
    public float[] times;

    // Start is called before the first frame update
    void OnEnable()
    {
        bool playerhaspos = false;
        for (int i = 0; i < names.Length; i++)
        {
            Text namepos =  transform.GetChild(i).GetComponent<Text>();
            Text timetext = namepos.transform.GetChild(0).GetComponent<Text>();
            if (timer.currentTime < times[i] && playerhaspos == false && timer.notWon == false)
            {
                playerhaspos = true;
                timetext.text = fixtime(timer.currentTime);
                timetext.fontStyle = FontStyle.Bold;
                namepos.text = (i+1).ToString() + "   " + "Lost Child";
                namepos.fontStyle = FontStyle.Bold;
            }
            else
            {
                timetext.text = fixtime(times[i]);
                namepos.text = (i+1).ToString()+"   " + names[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string fixtime(float time)
    {
    string min = ((int)time / 60).ToString();
    string sec = (time % 60).ToString("f0");
    string ost =  min + " minutes and " + sec + " seconds";
        return ost;
    }

}
