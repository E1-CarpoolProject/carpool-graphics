using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Material arrived_material;
    public Material waiting_material;
    private MeshRenderer passenger_mesh;

    void Awake()
    {
        passenger_mesh = this.gameObject.GetComponent<MeshRenderer>();
    }

    public void changeState(bool n)
    {
        if (n)
        {
            passenger_mesh.material = arrived_material;
        }
        else
        {
            passenger_mesh.material = waiting_material;
        }
    }
}
