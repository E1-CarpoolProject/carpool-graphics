using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Car
{
    public int new_direction;
}
public class MoveController : MonoBehaviour
{
    List<Vector3> next_Target;
    public Move[] cars;
    public float timeToUpdate;
    private float timer;
    
    IEnumerator ReceiveNextStep()
    {
        //[{"new_direction": 1}, {"new_direction": 2}, {"new_direction": 1}, {"new_direction": 2}]
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585/multiagentes";
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();          // Talk to Python
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);    // Answer from Python
            List<Vector3> newPositions = new List<Vector3>();
            string txt = www.downloadHandler.text.Replace('\'', '\"');
            txt = txt.TrimStart('[');
            txt = txt.TrimEnd(']');
            string[] strs = txt.Split(new string[] { "}, {" }, StringSplitOptions.None);
            //Debug.Log("strs.Length:" + strs.Length);
            for (int i = 0; i < strs.Length; i++)
            {
                strs[i] = strs[i].Trim();
                if (i == 0) strs[i] = strs[i] + '}';
                else if (i == strs.Length - 1) strs[i] = '{' + strs[i];
                else strs[i] = '{' + strs[i] + '}';
                //Debug.Log(strs[i]);
                Car a = JsonUtility.FromJson<Car>(strs[i]);
                //Debug.Log(a.new_direction);
                Vector3 new_target = fromIntToVector(a.new_direction);
                //Debug.Log(new_target);
                next_Target.Add(new_target);
            }
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].resetMove(next_Target[i]);
            }
        }

    }

    public Vector3 fromIntToVector(int number)
    {
        Vector3 movement;
        switch (number)
        {
            case 1: //UP
                movement = new Vector3(0, 0, 5);
                break;
            case 2: //RIGHT
                movement = new Vector3(5, 0, 0);
                break;
            case 3: //DOWN
                movement = new Vector3(0, 0, -5);
                break;
            case 4://LEFT
                movement = new Vector3(-5, 0, 0);
                break;
            default:
                movement = Vector3.zero;
                break;
        }
        return movement;
    }
    // Start is called before the first frame update
    void Start()
    {
        next_Target = new List<Vector3>();
        timer = timeToUpdate;
        StartCoroutine(ReceiveNextStep());
    }

    // Update is called once per frame
    void Update()
    {
        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            next_Target.Clear();
            StartCoroutine(ReceiveNextStep());
            timer = timeToUpdate;
        }
        
    }
}
