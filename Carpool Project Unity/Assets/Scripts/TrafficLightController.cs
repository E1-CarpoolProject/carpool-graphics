using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

[Serializable]
public class StopLight{
    public int state;
}

public class TrafficLightController : MonoBehaviour
{
    List<StopLight> stop_lights;
    public TrafficLight[] traffic_lights;
    public float timeToUpdate;
    private float timer;

    IEnumerator ReceiveNextStep(){
        //[{"state": 1}, {"state": 2}, {"state": 1}, {"state": 2}]
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585/traffic_lights";
        UnityWebRequest www = UnityWebRequest.Get(url);
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
                StopLight a = JsonUtility.FromJson<StopLight>(strs[i]);
                stop_lights.Add(a);
            }
            for (int i = 0; i < traffic_lights.Length; i++)
            {
                traffic_lights[i].state = stop_lights[i].state;
            }
        }
    }
    void Start()
    {
        stop_lights = new List<StopLight>();
        timer = timeToUpdate;
        //StartCoroutine(ReceiveNextStep());
    }

    // Update is called once per frame
    void Update()
    {
        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            stop_lights.Clear();
            StartCoroutine(ReceiveNextStep());
            timer = timeToUpdate;
        }
        
    }
}
