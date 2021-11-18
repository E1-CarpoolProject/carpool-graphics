using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacion : MonoBehaviour
{
    Dictionary<string, Vector3> vec3data;
    Dictionary<string, float> floatdata;

    // Start is called before the first frame update
    void Start()
    {
        vec3data = new Dictionary<string, Vector3>();
        floatdata = new Dictionary<string, float>();
        vec3data.Add("CENTRO", new Vector3(3.2f, 1.7f, -0.25f));
        vec3data.Add("LUZ", new Vector3(10, -4, 0.5f));
        vec3data.Add("CAMARA", new Vector3(-5, -3, 1));
        floatdata.Add("RADIO", 1.321f);
        floatdata.Add("ALPHA", 200);

        vec3data.Add("ka", new Vector3(0.08f, 0.01f, 0.02f));
        vec3data.Add("kd", new Vector3(0.88f, 0.15f, 0.21f));
        vec3data.Add("ks", new Vector3(0.88f, 0.88f, 0.88f));

        vec3data.Add("Ia", new Vector3(1, 1, 1));
        vec3data.Add("Id", new Vector3(1, 1, 1));
        vec3data.Add("Is", new Vector3(1, 1, 1));
        
        Vector3 PoI = vec3data["CENTRO"];
        PoI.y -= floatdata["RADIO"];
        vec3data.Add("PoI",PoI);
        Vector3 n = vec3data["PoI"] - vec3data["CENTRO"];
        vec3data.Add("N",n);
        Vector3 v = vec3data["CAMARA"] - vec3data["PoI"];
        vec3data.Add("V",v);
        Vector3 l = vec3data["LUZ"] -vec3data["PoI"];
        vec3data.Add("L",l);
        //r = lp-lo
        //lp =nu*dot(nu,l)
        //lo =l-lp
        float dotNuL = Vector3.Dot(n.normalized,l);
        Vector3 lp = n.normalized*dotNuL;
        Vector3 lo = l-lp;
        Vector3 r = lp-lo;
        vec3data.Add("R",r);
        float dotNuLu = Vector3.Dot(n.normalized, l.normalized);
        float dotVuRu = Vector3.Dot(v.normalized, r.normalized);
        float dotVuRuAlpha = Mathf.Pow(dotVuRu, floatdata["ALPHA"]);
        floatdata.Add("dotNuLu",dotNuLu);
        floatdata.Add("dotVuRu",dotVuRu);
        floatdata.Add("dotVuRuAlpha",dotVuRuAlpha);

        Vector3 ambient = Vector3.zero;
        ambient.x =vec3data["ka"].x *vec3data["Ia"].x;
        ambient.y =vec3data["ka"].y *vec3data["Ia"].y;
        ambient.z =vec3data["ka"].z *vec3data["Ia"].z;

        Vector3 diffuse = Vector3.zero;
        diffuse.x =vec3data["kd"].x *vec3data["Id"].x*dotNuLu;
        diffuse.y =vec3data["kd"].y *vec3data["Id"].y*dotNuLu;
        diffuse.z =vec3data["kd"].z *vec3data["Id"].z*dotNuLu;
        
        Vector3 specular = Vector3.zero;
        specular.x =vec3data["ks"].x *vec3data["Is"].x*dotVuRuAlpha;
        specular.y =vec3data["ks"].y *vec3data["Is"].y*dotVuRuAlpha;
        specular.z =vec3data["ks"].z *vec3data["Is"].z*dotVuRuAlpha;
        Vector3 finalColor = ambient +diffuse+specular;
        vec3data.Add("AMBIENT",ambient);
        vec3data.Add("DIFFUSE",diffuse);
        vec3data.Add("SPECULAR",specular);
        vec3data.Add("FINAL_COLOR",finalColor);
        
        foreach (KeyValuePair<string, Vector3> val in vec3data)
        {
            Debug.Log(val.Key + ": " + val.Value.ToString("F5"));
        }
        foreach (KeyValuePair<string, float> val in floatdata)
        {
            Debug.Log(val.Key + ": " + val.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(vec3data["PoI"], vec3data["PoI"]+vec3data["N"], Color.white);
        Debug.DrawLine(vec3data["PoI"], vec3data["PoI"]+vec3data["L"], Color.blue);
        Debug.DrawLine(vec3data["PoI"], vec3data["PoI"]+vec3data["V"], Color.red);
        Debug.DrawLine(vec3data["PoI"], vec3data["PoI"]+vec3data["R"], Color.green);

    }
}
