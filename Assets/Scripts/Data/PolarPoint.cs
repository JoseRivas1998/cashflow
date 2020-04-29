using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PolarPoint
{
    public float dist;
    public float theta;

    public PolarPoint(float dist, float theta)
    {
        this.dist = dist;
        this.theta = theta;
    }

    public Vector3 toVector3XZ
    {
        get
        {
            return new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)) * dist;
        }
    }

}
