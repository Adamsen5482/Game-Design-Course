using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafocus : MonoBehaviour
{
    public Transform target;
    public float speed;

    private void Update()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.right * Time.deltaTime*speed);
    }
}
