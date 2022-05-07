using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=7fdSYc8WElo
/// 
/// Handles the planetary orbit around the sun. 
/// 
/// </summary>

public class OrbitRotate : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject pivotObject;

    void Update()
    {
        // Rotates the gameobject around the pivotPoint, by the rotationSpeed
        transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
    }
}
