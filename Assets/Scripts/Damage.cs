using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private HealthSystem health;

    private void Start()
    {
        health = gameObject.GetComponentInParent<HealthSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            return;
        }

        ProjectileMovement proj = collision.gameObject.GetComponent<ProjectileMovement>();
        health.TakeDamage((int)proj.damage);
    }
}
