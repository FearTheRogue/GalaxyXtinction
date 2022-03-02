using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float fireRate;

    private void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;

        //Debug.Log("Hit: " + collision.gameObject.name);

        Destroy(gameObject);
    }
}
