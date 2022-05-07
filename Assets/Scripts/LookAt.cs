using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the gameobject looking at the player
/// 
/// </summary>

public class LookAt : MonoBehaviour
{
    [SerializeField] public GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        if(player != null)
            transform.LookAt(player.transform.position);
    }
}
