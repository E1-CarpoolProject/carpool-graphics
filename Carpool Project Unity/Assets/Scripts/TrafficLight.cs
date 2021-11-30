using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public Material red;
    public Material yellow;
    public Material green;
    public Material red_lighting;
    public Material yellow_lighting;
    public Material green_lighting;

    private MeshRenderer redLight;
    private MeshRenderer yellowLight;
    private MeshRenderer greenLight;

    public int state;
    public string id;
    void Start()
    {
        redLight = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        yellowLight = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();
        greenLight = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        state = 2;
    }

    void Update()
    {
        switch (state)
        {
            case 0:
                redLight.material = red_lighting;
                greenLight.material = green;
                yellowLight.material = yellow;
                break;
            case 1:
                redLight.material = red;
                greenLight.material = green;
                yellowLight.material = yellow_lighting;
                break;
            case 2:
                redLight.material = red;
                greenLight.material = green_lighting;
                yellowLight.material = yellow;
                break;
        }
    }
}
