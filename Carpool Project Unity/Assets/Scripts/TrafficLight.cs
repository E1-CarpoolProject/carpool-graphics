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

    private STATE state;

    public enum STATE
    {
        RED,
        YELLOW,
        GREEN
    }
    void Start()
    {
        redLight = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        yellowLight = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();
        greenLight = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        state = STATE.RED;
        StartCoroutine(changeLighting());
    }

    void Update()
    {

    }

    IEnumerator changeLighting()
    {
        while (true)
        {
            switch (state)
            {
                case STATE.RED:
                    redLight.material = red_lighting;
                    greenLight.material = green;
                    yellowLight.material = yellow;
                    state = STATE.YELLOW;
                    break;
                case STATE.YELLOW:
                    redLight.material = red;
                    greenLight.material = green;
                    yellowLight.material = yellow_lighting;
                    state = STATE.GREEN;
                    break;
                case STATE.GREEN:
                    redLight.material = red;
                    greenLight.material = green_lighting;
                    yellowLight.material = yellow;
                    state = STATE.RED;
                    break;
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
}
