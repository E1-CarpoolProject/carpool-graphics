using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angles : MonoBehaviour
{
    public Vector3 a;
    public Vector3 b;
    float angleAuB;
    void Start()
    {
        float result = Mathf.Atan2(b.z, a.x) * Mathf.Rad2Deg;
        Debug.Log("Angle between A and B is: " + result.ToString("F5"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
