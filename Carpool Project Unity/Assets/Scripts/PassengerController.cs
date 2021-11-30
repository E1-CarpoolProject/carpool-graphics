using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
[Serializable]
public class NewPassenger
{
    public int x;
    public int y;
    public int z;
    public bool arrived;
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
        string url = "http://localhost:8585/passengers";
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
                List<NewPassenger> new_passengers = JsonConvert.DeserializeObject<List<NewPassenger>>(www.downloadHandler.text);
                for (int i = 0; i < new_passengers.Count; i++)
                {
                    Debug.Log(new_passengers[i].arrived);
                    GameObject new_passenger = Instantiate(prefab, new Vector3(5 * new_passengers[i].x, 5 * new_passengers[i].y, 5 * new_passengers[i].z), Quaternion.identity);
                    new_passenger.GetComponent<Passenger>().changeState(new_passengers[i].arrived);
                    passengers.Add(new_passenger);
                }
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
