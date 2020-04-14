using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform HookPivot;
    public Transform Hookable;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centerPosition = new Vector3(HookPivot.transform.position.x + Hookable.transform.position.x, HookPivot.transform.position.y + Hookable.transform.position.y, HookPivot.transform.position.z + Hookable.transform.position.z)/2f; 
        transform.position = centerPosition;

        transform.rotation = Quaternion.LookRotation(Hookable.transform.position - transform.position);

        float dist = Vector3.Distance(HookPivot.transform.position, Hookable.transform.position); 

        transform.localScale = new Vector3(0.1f, 0.1f, dist);

    }
}
