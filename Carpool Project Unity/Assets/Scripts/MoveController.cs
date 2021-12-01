using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
[Serializable]
public class NewDirection
{
    public string next_direction;
}

public class MoveController : MonoBehaviour
{
    List<Vector3> next_Target;
    public List<Move> cars;
    public GameObject[] prefabs;
    public float timeToUpdate;
    private float timer;
    
    IEnumerator GenerateNewCars()
    {
        //[{"new_direction": 1}, {"new_direction": 2}, {"new_direction": 1}, {"new_direction": 2}]
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "https://smaa01653126.us-south.cf.appdomain.cloud/new_cars";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();          // Talk to Python
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);    // Answer from Python
            if (www.downloadHandler.text != "[]")
            {
                List<Vector3> new_cars = JsonConvert.DeserializeObject<List<Vector3>>(www.downloadHandler.text);
                for (int i = 0; i < new_cars.Count; i++)
                {
                    System.Random rd = new System.Random();
                    int random_car = rd.Next(0, prefabs.Length);
                    GameObject new_car = Instantiate(prefabs[random_car], new Vector3(5 * new_cars[i].x, 5 * new_cars[i].y, 5 * new_cars[i].z), Quaternion.identity);
                    //Debug.LogWarning(new_car.transform.position);
                    cars.Add(new_car.GetComponent<Move>());
                }
            }
        }
        StartCoroutine(ReceiveNextStep());
    }
    IEnumerator ReceiveNextStep()
    {
        //[{"new_direction": 1}, {"new_direction": 2}, {"new_direction": 1}, {"new_direction": 2}]
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "https://smaa01653126.us-south.cf.appdomain.cloud/directions";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();          // Talk to Python
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);    // Answer from Python
            if (www.downloadHandler.text != "[]")
            {
                List <NewDirection> new_directions = JsonConvert.DeserializeObject<List<NewDirection>>(www.downloadHandler.text);
                for (int i = 0; i < new_directions.Count; i++)
                {
                    if (new_directions[i].next_direction == "PA")
                    {
                        cars[i].gameObject.SetActive(false);
                    }
                    //Debug.Log(new_directions[i].next_direction);
                    Vector3 new_target = fromStringToVector(new_directions[i].next_direction);
                    next_Target.Add(new_target);
                }
                for (int i = 0; i < cars.Count; i++)
                {
                    cars[i].resetMove(next_Target[i]);
                }
            }
        }
    }

    public Vector3 fromStringToVector(string direction)
    {
        Vector3 movement;
        switch (direction)
        {
            case "UP": //UP
                movement = new Vector3(0, 0, 5);
                break;
            case "RH": //RIGHT
                movement = new Vector3(5, 0, 0);
                break;
            case "DW": //DOWN
                movement = new Vector3(0, 0, -5);
                break;
            case "LF"://LEFT
                movement = new Vector3(-5, 0, 0);
                break;
            default:
                movement = Vector3.zero;
                break;
        }
        return movement;
    }

    void Start()
    {
        next_Target = new List<Vector3>();
        timer = timeToUpdate;
        //StartCoroutine(ReceiveNextStep());
    }

    void Update()
    {
        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            next_Target.Clear();
            StartCoroutine(GenerateNewCars());
            timer = timeToUpdate;
        }
        
    }
}
