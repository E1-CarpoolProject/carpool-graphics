using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject car;
    public Vector3 source, target;
    public float totalTime, timer;

    private float dt;
    private float angleAuB;
    private Vector3 currPos, targetDir;
    private Vector3[] geometry;
    private MeshFilter mf;
    private Mesh mesh;

    void Awake()
    {
        mf = car.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        geometry = mesh.vertices;
        timer = totalTime;
        currPos = source;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        dt = 1.0f - (timer / totalTime);
        if (timer > 0)
        {
            currPos = source + (dt * (target - source));
            targetDir = target - currPos;
            angleAuB = Mathf.Atan2(targetDir.z, targetDir.x) * Mathf.Rad2Deg;
            mesh.vertices = ApplyTransformation(currPos);
        }
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

    public void resetMove(Vector3 newTarget)
    {
        mesh.RecalculateBounds();
        source = target;
        target = newTarget + source;
        timer = totalTime;
    }
}
