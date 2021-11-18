using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformations
{
    public enum AXIS    // An enumeration is useful to remember names instead of numbers
    {
        AX_X,           // Value will be set to 0 automatically
        AX_Y,           // Value will be set to 1 automatically
        AX_Z            // Value will be set to 2 automatically
    }

    public static Matrix4x4 TranslateM(float tx, float ty, float tz)
    {
        Matrix4x4 tm = Matrix4x4.identity;
        tm[0, 3] = tx;
        tm[1, 3] = ty;
        tm[2, 3] = tz;
        return tm;
    }

    public static Matrix4x4 ScaleM(float sx, float sy, float sz)
    {
        Matrix4x4 sm = Matrix4x4.identity;
        sm[0, 0] = sx;
        sm[1, 1] = sy;
        sm[2, 2] = sz;
        return sm;
    }

    public static Matrix4x4 RotateM(float angle, AXIS axis)
    {
        Matrix4x4 rm = Matrix4x4.identity;
        float rad = angle * Mathf.Deg2Rad;
        if (axis == AXIS.AX_X)
        {
            rm[1, 1] = Mathf.Cos(rad);
            rm[1, 2] = -Mathf.Sin(rad);
            rm[2, 1] = Mathf.Sin(rad);
            rm[2, 2] = Mathf.Cos(rad);
        }
        else if (axis == AXIS.AX_Y)
        {
            rm[0, 0] = Mathf.Cos(rad);
            rm[0, 2] = Mathf.Sin(rad);
            rm[2, 0] = -Mathf.Sin(rad);
            rm[2, 2] = Mathf.Cos(rad);
        }
        else if (axis == AXIS.AX_Z)
        {
            rm[0, 0] = Mathf.Cos(rad);
            rm[0, 1] = -Mathf.Sin(rad);
            rm[1, 0] = Mathf.Sin(rad);
            rm[1, 1] = Mathf.Cos(rad);
        }
        return rm;
    }

}





