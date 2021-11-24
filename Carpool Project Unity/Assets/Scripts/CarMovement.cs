using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public enum DIRECTION
    {
        FORWARD,
        RIGHT,
        LEFT
    }
    //El modelo de los autos siempre apunta hacia "z"
    public GameObject car;
    public Vector3 source, goal;
    public DIRECTION direction;
    public int steps;

    private float dt;
    private Vector3[] geometry;
    private MeshFilter mf;
    private Mesh mesh;
    private float currAngleRot;
    private float angleAuB;
    void Start()
    {
        mf = car.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        geometry = mesh.vertices;
        dt = 0f;
        currAngleRot = 0f;
        angleAuB = Mathf.Atan2(goal.z, source.x) * Mathf.Rad2Deg;
        Debug.Log(angleAuB);
    }

    void Update()
    {
        
    }

    public Vector3[] ApplyTranslation(float t)
    {
        Vector3 move = source + t * (goal - source);     //Interpolacion lineal
        Matrix4x4 translation = Transformations.TranslateM(move.x, move.y, move.z);
        Matrix4x4 rotation = Transformations.RotateM(currAngleRot % 360, Transformations.AXIS.AX_Y);
        Vector3[] transform = new Vector3[geometry.Length];
        for (int i = 0; i < geometry.Length; i++)
        {
            Vector3 v = geometry[i];
            Vector4 temp = new Vector4(v.x, v.y, v.z, 1);
            transform[i] = translation * rotation * temp;
            //Debug.Log(transform[i]);
        }
        return transform;
    }

    public bool keepMoving()
    {
        if (dt <= 1f) { return true; };
        return false;
    }
    public void moveCar()
    {
        mf = car.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        if (keepMoving())
        {
            dt += (float)1 / steps;
            if (goal.z == source.z || source.x == goal.x)
            {
                direction = DIRECTION.FORWARD;
                currAngleRot += 0;
            }
            else
            {
                if (angleAuB > 90)
                {
                    direction = DIRECTION.RIGHT;
                    currAngleRot += (float)90 / steps;
                }
                else if (angleAuB < 90)
                {
                    direction = DIRECTION.LEFT;
                    currAngleRot -= (float)90 / steps;
                }
            }
            mesh.vertices = ApplyTranslation(dt);
        }
    }
    public void resetMovement()
    {
        mf = car.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        geometry = mesh.vertices;
        dt = 0f;
        currAngleRot = 0f;
        angleAuB = Mathf.Atan2(source.z, goal.x) * Mathf.Rad2Deg;
        Debug.Log(angleAuB);
    }
}
