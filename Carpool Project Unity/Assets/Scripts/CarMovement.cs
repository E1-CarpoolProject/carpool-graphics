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
    private float totalAngleRotation;
    void Start()
    {
        mf = car.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        geometry = mesh.vertices;
        dt = 0f;
        currAngleRot = 0f;
        Debug.Log("Source: " + source.ToString());
        Debug.Log("Goal: " + goal.ToString());
        if (goal.x == source.x || goal.z == source.z)
        {
            direction = DIRECTION.FORWARD;
        }
        else if (goal.magnitude < source.magnitude)
        {
            if ((Mathf.Acos(Vector3.Dot(source.normalized, goal.normalized - source.normalized)) * Mathf.Rad2Deg) > 90f)
            {
                direction = DIRECTION.LEFT;
            }
            else if ((Mathf.Acos(Vector3.Dot(source.normalized, goal.normalized - source.normalized)) * Mathf.Rad2Deg) < 90f)
            {
                direction = DIRECTION.RIGHT;
            }
        }
        else
        {
            if ((Mathf.Acos(Vector3.Dot(source.normalized, source.normalized - goal.normalized)) * Mathf.Rad2Deg) > 90f)
            {
                direction = DIRECTION.LEFT;
            }
            else if ((Mathf.Acos(Vector3.Dot(source.normalized, source.normalized - goal.normalized)) * Mathf.Rad2Deg) < 90f)
            {
                direction = DIRECTION.RIGHT;
            }
        }
    }

    void Update()
    {
        /*
        if (keepMoving())
        {
            moveCar();
        };
        */
    }

    public Vector3[] ApplyTranslation(float t)
    {
        Vector3 move = source + t * (goal - source);     //Interpolacion lineal
        Matrix4x4 translation = Transformations.TranslateM(move.x, move.y, move.z);
        Matrix4x4 translationOrigin = Transformations.TranslateM(-source.x, -source.y, -source.z);
        Matrix4x4 rotation = Transformations.RotateM(currAngleRot % 360, Transformations.AXIS.AX_Y);
        Vector3[] transform = new Vector3[geometry.Length];
        for (int i = 0; i < geometry.Length; i++)
        {
            Vector3 v = geometry[i];
            Vector4 temp = new Vector4(v.x, v.y, v.z, 1);
            transform[i] = translation * translationOrigin * rotation * temp;
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
        if (dt <= 1f)
        {
            dt += (float)1 / steps;
            if (direction == DIRECTION.LEFT)
            {
                currAngleRot -= (float)90 / steps;
            }
            else if (direction == DIRECTION.RIGHT)
            {
                currAngleRot += (float)90 / steps;
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
        Debug.Log("Source: " + source.ToString());
        Debug.Log("Goal: " + goal.ToString());
        if (goal.x == source.x || goal.z == source.z)
        {
            direction = DIRECTION.FORWARD;
        }
        else if (goal.magnitude < source.magnitude)
        {
            if ((Mathf.Acos(Vector3.Dot(source.normalized, goal.normalized - source.normalized)) * Mathf.Rad2Deg) > 90f)
            {
                direction = DIRECTION.LEFT;
            }
            else if ((Mathf.Acos(Vector3.Dot(source.normalized, goal.normalized - source.normalized)) * Mathf.Rad2Deg) < 90f)
            {
                direction = DIRECTION.RIGHT;
            }
        }
        else
        {
            if ((Mathf.Acos(Vector3.Dot(source.normalized, source.normalized - goal.normalized)) * Mathf.Rad2Deg) > 90f)
            {
                direction = DIRECTION.LEFT;
            }
            else if ((Mathf.Acos(Vector3.Dot(source.normalized, source.normalized - goal.normalized)) * Mathf.Rad2Deg) < 90f)
            {
                direction = DIRECTION.RIGHT;
            }
        }
    }
}
