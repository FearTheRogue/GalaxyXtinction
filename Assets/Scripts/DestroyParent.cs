using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handle the destruction of a gameobjects parent, specified by a float
/// 
/// </summary>

public class DestroyParent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected orb");
            Destroy(transform.gameObject);
        }
    }
}
