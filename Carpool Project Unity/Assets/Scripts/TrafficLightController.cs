using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

[Serializable]
public class StopLight{
    public int state;
    public string id;
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
            //Debug.Log(www.downloadHandler.text);    // Answer from Python
            List <StopLight> semaforos = JsonConvert.DeserializeObject<List<StopLight>>(www.downloadHandler.text);
            for (int i = 0; i < semaforos.Count; i++)
            {
                //Debug.Log(semaforos[i].state);
                string s = semaforos[i].id;
                for (int j = 0; j < traffic_lights.Length; j++)
                {
                    //Debug.Log("ID JSON: " + semaforos[i].id + " ID LOCAL: " + traffic_lights[i].id);
                    if (traffic_lights[j].id == s)
                    {
                        traffic_lights[j].changeState(semaforos[i].state);
                        break;
                    }
                }
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
