using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the destruction of a gameobject, specified by a float
/// 
/// </summary>

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;

    private void Update()
    {
        Destroy(this.gameObject, timeToDestroy);
    }
}
