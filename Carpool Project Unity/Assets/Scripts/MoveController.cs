using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class NewDirection
{
    public int new_direction;
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
        string url = "http://localhost:8585/directions";
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();          // Talk to Python
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);    // Answer from Python
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
                Vector3 a = JsonUtility.FromJson<Vector3>(strs[i]);
                System.Random rd = new System.Random();
                int random_car = rd.Next(0, prefabs.Length);
                GameObject new_car = Instantiate(prefabs[random_car], new Vector3(5 * a.x, 5 * a.y, 5 * a.z), Quaternion.identity);
                cars.Add(new_car.GetComponent<Move>());
            }
        }
    }
    IEnumerator ReceiveNextStep()
    {
        //[{"new_direction": 1}, {"new_direction": 2}, {"new_direction": 1}, {"new_direction": 2}]
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585/directions";
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();          // Talk to Python
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);    // Answer from Python
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
                NewDirection a = JsonUtility.FromJson<NewDirection>(strs[i]);
                //Debug.Log(a.new_direction);
                Vector3 new_target = fromIntToVector(a.new_direction);
                //Debug.Log(new_target);
                next_Target.Add(new_target);
            }
            for (int i = 0; i < cars.Count; i++)
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

    private void Awake()
    {
        
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
