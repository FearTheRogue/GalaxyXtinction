using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timeToDespawn = 10f;

    public float projectileSpeed = 100f;

    void Start()
    {

    }

    private void Update()
    {
       Destroy(this.gameObject, timeToDespawn);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit Object: " + other);
    }
}
