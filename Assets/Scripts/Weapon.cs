using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { Burst, Auto };

public class Weapon : WeaponManager
{
    [SerializeField] private string weaponName;

    [Header("Weapon Properties")]
    [SerializeField] private FireMode fireMode = FireMode.Auto;
    [SerializeField] private Transform[] missileSpawnPoint;
    [SerializeField] private Transform[] homingMissileSpawnPoint;
    
    [SerializeField] private GameObject primaryWeapon;
    [SerializeField] private GameObject secondaryWeapon;

    [Header("Missile Settings")]
    [SerializeField] private int burstAmount;
    [SerializeField] private float fireTimer = 0;
    [SerializeField] private float fireRate;
    [SerializeField] private bool canBurstFire;

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
        if (Time.time > fireTimer && !canBurstFire)
        {
            if (fireMode == FireMode.Auto)
            {
                AudioManager.instance.PlayOnce("Weapon Shoot");
                Shoot();
            }

            if(fireMode == FireMode.Burst)
            {
                canBurstFire = true;

                StartCoroutine(BurstFire());
            }

            fireTimer = Time.time + fireRate;
        } 
        else 
        { 
            return;
        }
    }

    private void Shoot()
    {
        foreach (Transform origin in missileSpawnPoint)
        {
            GameObject projectile = Instantiate(primaryWeapon, origin.transform.position, origin.transform.rotation);
            var flash = projectile.GetComponent<ProjectileMovement>();

            //if (flash.GetMuzzelFlashObject() != null)
            //{
            //    Instantiate(flash.GetMuzzelFlashObject().gameObject, origin.transform.position, transform.rotation, origin.transform);
            //}

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), this.transform.parent.GetComponent<Collider>());
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

    public int GetNumOfWeapons()
    {
        if(primaryWeapon != null && secondaryWeapon != null)
        {
            return 2;
        }
        else if (secondaryWeapon == null)
        {
            return 1;
        }

        Debug.LogWarning(gameObject.transform.name + " does not have any weapons attached");
        return 0;
    }

    private IEnumerator BurstFire()
    {
        yield return new WaitForSeconds(fireRate);

        for (int i = 0; i < burstAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }

        canBurstFire = false;
    }
}
