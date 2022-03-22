using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject muzzelFlash;
    [SerializeField] private GameObject trail;

    [Header("Details")]
    [SerializeField] private float speed;
    [SerializeField] private float accuracy;
    [SerializeField] private float damage;

    private Vector3 startPosition;
    private Vector3 offset;
    private Rigidbody rb;

    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        if (trail == null)
            return;
    }

    private void Start()
    {
        if(accuracy != 100)
        {
            accuracy = 1 - (accuracy / 100);

            for (int i = 0; i < 2; i++)
            {
                float value = 1 * Random.Range(-accuracy, accuracy);
                float index = Random.Range(0, 2);

                if(i == 0)
                {
                    if (i == 0)
                        offset = new Vector3(0, -value, 0);
                    else
                        offset = new Vector3(0, value, 0);
                } 
                else
                {
                    if (index == 0)
                        offset = new Vector3(0, offset.y, -value);
                    else
                        offset = new Vector3(0, offset.y, value);
                }
            }
        }
    }

    private void Update()
    {
        if(speed != 0 && rb != null)
        {
            rb.position += (transform.forward + offset) * (speed * Time.deltaTime);
        }
    }

    public GameObject GetMuzzelFlashObject()
    {
        return muzzelFlash.gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Projectile")
        {
            Debug.Log("Projectile has hit: " + collision.transform.root.name);

            HealthSystem healthSystem = collision.transform.root.GetComponent<HealthSystem>();

            if(healthSystem != null)
            {
                healthSystem.TakeDamage((int)damage);
                DestroyProjectile();
            }
            else
            {
                return;
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
