using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { Semi, Burst, Auto };

public class Weapon : WeaponManager
{
    [SerializeField] private string weaponName;

    [Header("Weapon Properties")]
    [SerializeField] private FireMode fireMode = FireMode.Semi;
    [SerializeField] private Transform[] missileSpawnPoint;
    [SerializeField] private Transform[] homingMissileSpawnPoint;
    
    [SerializeField] private GameObject primaryWeapon;
    [SerializeField] private GameObject secondaryWeapon;

    [Header("Missile Settings")]
    [SerializeField] private int burstAmount, maxBurstAmount;
    [SerializeField] private float rechargeTime;

    [SerializeField] private float fireTimer = 0;
    [SerializeField] private float fireRate;

    [SerializeField] private bool burstFire;

    [Header("Homing Missile Settings")]
    [SerializeField] private bool canHomingMissile = true;
    [SerializeField] private float homingCooldown;
    [SerializeField] private float maxHomingCooldown;

    private void Awake()
    {
        if (!secondaryWeapon)
            return;
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

        if (!canHomingMissile)
        {
            homingCooldown = Mathf.MoveTowards(homingCooldown, maxHomingCooldown, (1 * Time.deltaTime));

            if (homingCooldown == maxHomingCooldown)
                canHomingMissile = true;
        }
    }

    public float GetAttackRange()
    {
        // return attackRange;
        return 0;
    }

    public void ShootMissile()
    {
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
                    GameObject projectile = Instantiate(primaryWeapon, origin.transform.position, origin.transform.rotation);

                    Physics.IgnoreCollision(projectile.GetComponent<Collider>(), this.transform.parent.GetComponent<Collider>());
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
                        Instantiate(primaryWeapon, origin.transform.position, origin.transform.rotation);
                    }

                    burstAmount++;
                }

                fireTimer = 0f;
            }
        }
    }

    public void ShootHomingMissile()
    {
        if (canHomingMissile)
        {
            canHomingMissile = false;
            homingCooldown = 0f;
            StartCoroutine(FiringHomingMissiles());
        }
    }

    private IEnumerator FiringHomingMissiles()
    {
        foreach (Transform origin in homingMissileSpawnPoint)
        {
            GameObject projectile = Instantiate(secondaryWeapon, origin.transform.position, origin.transform.rotation);
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), this.transform.parent.GetComponent<Collider>());
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void BurstShoot()
    {
        if (burstAmount == 0)
            return;

        for (int i = 0; i <= maxBurstAmount; i++)
        {
            ShootMissile();
            burstAmount--;
        }
    }
}
