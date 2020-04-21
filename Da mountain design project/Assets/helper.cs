using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class helper : MonoBehaviour
{
    public static helper instance = null;
    Text[] containers;
    Coroutine[] Coroutines;
    List<string> pastMessages;
    

    void Awake()
    {

        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

    }

    private void Start()
    {
        pastMessages = new List<string>();
        containers = new Text[3];
        Coroutines = new Coroutine[3];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform g = transform.GetChild(i);
            containers[i] = g.GetComponent<Text>();
        }
    
        StartCoroutine(GetMessage("Press 'E' to Climb")); 
        StartCoroutine(GetMessage("Press 'Space' to Jump")); 

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) {
            RemoveMessage("Press 'E' to Climb");
        }

        if (Input.GetKey(KeyCode.Space)) {
            RemoveMessage("Press 'Space' to Jump");
        }
      
    }

    bool check(string message)
    {
         for (int i = 0; i < pastMessages.Count; i++)
        {
            if (message == pastMessages[i])
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator GetMessage(string message)
    {

        if (!check(message))
        {


            pastMessages.Add(message);
            bool found = false;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].text == "")
                {
                    found = true;
                    Coroutines[i] = StartCoroutine(MessageTimer(containers[i], message));
                    break;
                }
            }
            if (!found)
            {
                yield return new WaitForSeconds(5);
                StartCoroutine(GetMessage(message));
            }
        }
    }

    public void RemoveMessage(string message)
    {
        
        for (int i = 0; i < containers.Length; i++)
        {
            if (containers[i].text == message)
            {
                StopCoroutine(Coroutines[i]);
                containers[i].text = "";

                break;
            }
        }
    }

    IEnumerator MessageTimer(Text target, string message)
    {
        target.text = message;
       // target.GetComponent<Animation>().Play();
       yield return new WaitForSeconds(20);
        target.text = "";
    }


}
