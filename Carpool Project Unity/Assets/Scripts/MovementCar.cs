using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCar : MonoBehaviour
{
    public GameObject car;
    public List<Vector3> pointsToVisit;
    public float totalTime;

    private int segment;
    private float timer, dt;
    private Vector3[] geometry;
    private MeshFilter mf;
    private Mesh mesh;
    private float angleAuB;
    private Vector3 currPos;
    private Vector3 prevPos;
    void Start()
    {
        mf = car.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        geometry = mesh.vertices;
        timer = totalTime;
        segment = 0;
        prevPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        dt = 1.0f - (timer / totalTime);
        if (timer > 0)
        {
            if (dt < 0.25f) segment = 0;
            else if (dt < 0.5f) segment = 1;
            else if (dt < 0.75f) segment = 2;
            else if (dt < 1.0f) segment = 3;

            currPos = Lerp(pointsToVisit[segment], pointsToVisit[segment + 1], dt, segment);
            //A donde voy y cual es la posicion actual
            Vector3 targetDir = pointsToVisit[segment + 1] - currPos;
            angleAuB = Mathf.Atan2(targetDir.z, targetDir.x) * Mathf.Rad2Deg;
            Debug.Log("Dir: " + targetDir.ToString("F5"));
            Debug.Log(angleAuB);
            prevPos = currPos;
            mf = car.GetComponent<MeshFilter>();
            mesh = mf.mesh;
            mesh.vertices = ApplyTransformation(currPos);
        }
    }

    Vector3 Lerp(Vector3 source, Vector3 target, float t, int s)
    {
        float q = (t * 4) - s;
        return source + q*(target - source);
    }

    public Vector3[] ApplyTransformation(Vector3 pos)
    {
        Matrix4x4 translation = Transformations.TranslateM(pos.x, pos.y, pos.z);
        Matrix4x4 rotation = Transformations.RotateM(angleAuB, Transformations.AXIS.AX_Y);
        Vector3[] transform = new Vector3[geometry.Length];
        for (int i = 0; i < geometry.Length; i++)
        {
            Vector3 v = geometry[i];
            Vector4 temp = new Vector4(v.x, v.y, v.z, 1);
            transform[i] = translation * rotation * temp;
        }
        return transform;
    }

    public Vector3[] Rotate(float angle)
    {
        return null;
    }
}
