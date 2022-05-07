using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=mQKGRoV_jBc&t=695s
/// 
/// This script was not used in the project.
/// 
/// </summary>

[System.Serializable]
public class Ellipse
{
    public float xAxis;
    public float yAxis;

    public Ellipse(float xAxis, float yAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }

    public Vector2 Evaluate (float t)
    {
        float angle = Mathf.Deg2Rad * 360 * t;
        float xPos = Mathf.Sin(angle) * xAxis;
        float yPos = Mathf.Cos(angle) * yAxis;

        return new Vector2(xPos, yPos);
    }
}
