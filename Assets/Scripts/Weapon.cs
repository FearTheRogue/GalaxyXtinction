using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { Semi, Burst, Auto };

public class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    [SerializeField] private FireMode fireMode = FireMode.Semi;
    [SerializeField] private Transform[] missileSpawnPoint;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int burstAmount, maxBurstAmount;
    [SerializeField] private float rechargeTime;

    [SerializeField] private float fireTimer = 0;
    [SerializeField] private float fireRate;

    [SerializeField] private bool burstFire;

    private void Awake()
    {
        //burstAmount = maxBurstAmount;
    }

    private void Update()
    {
        switch (fireMode)
        {
            case FireMode.Semi:
                fireMode = FireMode.Semi;
                break;
            case FireMode.Burst:
                fireMode = FireMode.Burst;
                break;
            case FireMode.Auto:
                fireMode = FireMode.Auto;
                break;
        }
    }

    public float GetAttackRange()
    {
        // return attackRange;
        return 0;
    }

    public void Shoot()
    {
        Debug.Log("Shoot was called");

        if(fireTimer < fireRate + 1.0f)
        {
            fireTimer += Time.deltaTime;
        }

        if (fireMode == FireMode.Auto)
        {
            if (fireTimer > fireRate)
            {
                foreach (Transform origin in missileSpawnPoint)
                {
                    GameObject projectile = Instantiate(missilePrefab, origin.transform.position, origin.transform.rotation);

                    Physics.IgnoreCollision(projectile.GetComponent<Collider>(), origin.parent.GetComponent<Collider>());
                }

                fireTimer = 0f;
            }
        }

        if(fireMode == FireMode.Burst)
        {
            if (fireTimer > fireRate)
            {
                if (burstAmount >= maxBurstAmount)
                {
                    foreach (Transform origin in missileSpawnPoint)
                    {
                        Instantiate(missilePrefab, origin.transform.position, origin.transform.rotation);
                    }

                    burstAmount++;
                }
                fireTimer = 0f;

            }
        }
    }

    public void BurstShoot()
    {
        if (burstAmount == 0)
            return;

        for (int i = 0; i <= maxBurstAmount; i++)
        {
            Shoot();
            burstAmount--;
        }
    }
}
