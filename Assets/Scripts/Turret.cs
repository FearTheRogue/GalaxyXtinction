using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject rotatePivot;
    [SerializeField] private GameObject anglePivot;
    [SerializeField] private GameObject firePoint;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Vector3 startingLocation;
    [SerializeField] private Vector3 startingRotation;

    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float angleSpeed;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject target;
    Quaternion rotation;
    Vector3 direction;

    [SerializeField] private Weapon weapon;
    [SerializeField] private float fireTimer;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject projectile;

    private void Start()
    {
        startingLocation = anglePivot.transform.position;
        startingRotation = rotatePivot.transform.localEulerAngles;
    }

    private void Update()
    {
        if (target == null)
        {
            ResetPosition();
            return;
        }

        MoveToTarget();
    }

    private void ResetPosition()
    {
        UpdateRotationBase(startingLocation);
        UpdateAngle(startingLocation);
    }

    private void MoveToTarget()
    {
        UpdateRotationBase(target.transform.position);
        UpdateAngle(target.transform.position);

        if (CheckCanShoot())
        {
            if (Time.time > fireTimer)
            {
                ShootMissile();

                fireTimer = Time.time + fireRate;
            }
        }
    }

    private void UpdateRotationBase(Vector3 position)
    {
        direction = (position - rotatePivot.transform.position).normalized;
        rotation = Quaternion.LookRotation(direction);

        rotatePivot.transform.rotation = Quaternion.Slerp(rotatePivot.transform.rotation, rotation, (rotateSpeed * Time.deltaTime));
        rotatePivot.transform.rotation = Quaternion.Euler(new Vector3(0f, rotatePivot.transform.rotation.eulerAngles.y, 0f));
    }

    private void UpdateAngle(Vector3 position)
    {
        direction = (position - anglePivot.transform.position).normalized;
        rotation = Quaternion.LookRotation(direction);

        anglePivot.transform.rotation = Quaternion.Slerp(anglePivot.transform.rotation, rotation, (angleSpeed * Time.deltaTime));
        anglePivot.transform.rotation = Quaternion.Euler(new Vector3(anglePivot.transform.rotation.eulerAngles.x, rotatePivot.transform.rotation.eulerAngles.y, 0f));

        Vector3 angle = anglePivot.transform.eulerAngles;

        angle.x = Mathf.Clamp(anglePivot.transform.eulerAngles.x, angleMin, angleMax);
        anglePivot.transform.rotation = Quaternion.Euler(angle);
    }

    private bool CheckCanShoot()
    {
        if (target == null)
            return false;

        RaycastHit hit;

        if(Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, 100,layerMask))
        {
            return true;
        }

        Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward, Color.green);
        return false;
    }

    public void ShootMissile()
    {
        GameObject proj = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);

        //Physics.IgnoreCollision(proj.GetComponent<Collider>(), transform.GetComponentsInChildren<BoxCollider>().;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        target = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        target = null;
    }
}
