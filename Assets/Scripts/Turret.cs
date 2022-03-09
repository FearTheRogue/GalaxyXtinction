using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject rotatePivot;
    [SerializeField] private GameObject anglePivot;

    [SerializeField] private Vector3 startingLocation;
    [SerializeField] private Quaternion startingRotation;

    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float angleSpeed;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 target;
    Quaternion rotation;
    Vector3 direction;

    private void Start()
    {
        startingLocation = this.transform.position;
        startingRotation = this.transform.rotation;
    }

    private void Update()
    {
        if (target == null)
        {
            //target.transform.position = startingLocation;
            //target.transform.rotation = startingRotation;
        }

        UpdateRotationBase();
        UpdateAngle();
    }

    private void UpdateRotationBase()
    {
        direction = (target - rotatePivot.transform.position).normalized;
        rotation = Quaternion.LookRotation(direction);

        rotatePivot.transform.rotation = Quaternion.Slerp(rotatePivot.transform.rotation, rotation, (rotateSpeed * Time.deltaTime));
        rotatePivot.transform.rotation = Quaternion.Euler(new Vector3(0f, rotatePivot.transform.rotation.eulerAngles.y, 0f));
    }

    private void UpdateAngle()
    {
        direction = (target - anglePivot.transform.position).normalized;
        rotation = Quaternion.LookRotation(direction);

        anglePivot.transform.rotation = Quaternion.Slerp(anglePivot.transform.rotation, rotation, (angleSpeed * Time.deltaTime));
        anglePivot.transform.rotation = Quaternion.Euler(new Vector3(anglePivot.transform.rotation.eulerAngles.x, rotatePivot.transform.rotation.eulerAngles.y, 0f));

        Vector3 angle = anglePivot.transform.eulerAngles;

        angle.x = Mathf.Clamp(anglePivot.transform.eulerAngles.x, angleMin, angleMax);
        anglePivot.transform.rotation = Quaternion.Euler(angle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        target = other.gameObject.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
    }
}
