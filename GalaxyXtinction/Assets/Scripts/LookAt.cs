using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] public Transform player;

    private void FixedUpdate()
    {
        transform.LookAt(player);
    }
}
