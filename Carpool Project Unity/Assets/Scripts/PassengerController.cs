using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class NewPassenger
{
    public int x;
    public int y;
    public int z;
    public bool hasArrived;
}
public class PassengerController : MonoBehaviour
{
    public GameObject prefab;
    public float timerToUpdate;

    private List<GameObject> passengers;
    private float timer;
    IEnumerator GeneratePassenger()
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
                Debug.Log(strs[i]);
                NewPassenger a = JsonUtility.FromJson<NewPassenger>(strs[i]);
                GameObject new_passenger = Instantiate(prefab, new Vector3(5 * a.x, 5 * a.y, 5 * a.z), Quaternion.identity);
                new_passenger.GetComponent<Passenger>().changeState(a.hasArrived);
                passengers.Add(new_passenger);
            }
        }
    }
    void Start()
    {
        passengers = new List<GameObject>();
        timer = timerToUpdate;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            for (int i = 0; i < passengers.Count; i++)
            {
                Destroy(passengers[i]);
            }
            passengers.Clear();
            StartCoroutine(GeneratePassenger());
            timer = timerToUpdate;
        }
    }
}
