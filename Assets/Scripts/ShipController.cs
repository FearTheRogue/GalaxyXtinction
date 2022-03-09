using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private AnimationCurve camCurve;
    [SerializeField] private float startFOV;
    [SerializeField] private float normalSpeedFOV;
    [SerializeField] private float boostSpeedFOV;
    [SerializeField] private float fovT;

    [Header("Ship Settings")]
    [SerializeField] private Transform[] pointOfOrigin;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Weapon weapon;
    
    [Header("Normal Speed Settings")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float forwardAcceleration;

    [Header("Thruster Speed Settings")]
    [SerializeField] private ThrusterBar thrusters;
    [SerializeField] private float thrusterSpeed;
    [SerializeField] private float thrusterStock;
    [SerializeField] private float maxThrusterStock;
    [SerializeField] private float decreaseThrusterAmount;
    [SerializeField] private float IncreaseThrusterAmount;
    [SerializeField] private float rechargeDelay;
    [SerializeField] private bool isThrusting;

    private float roll;
    private float rollSpeed = 90f;
    private float rollAcceleration = 5f;

    private Vector2 mouseLookInput, screenCenter, mouseDistance;

    public Rigidbody Rb => rb;
    
    void Start()
    {
        // Finding the screen center
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;

        weapon = GetComponent<Weapon>();

        thrusterStock = maxThrusterStock;
        thrusters.SetMaxThrusterValue(thrusterStock);
    }

    void Update()
    {
        mouseLookInput.x = Input.mousePosition.x;
        mouseLookInput.y = Input.mousePosition.y;

        mouseDistance.x = (mouseLookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (mouseLookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        RollShip();

        //if (Input.GetAxisRaw("Vertical") >= 0.5f && Input.GetAxisRaw("Vertical") <= 1f)
        //    cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, normalSpeedFOV, camCurve.Evaluate(fovT));
        //else
        //    cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, startFOV, camCurve.Evaluate(fovT));

        isThrusting = IsThrusting();

        if (IsThrusting())
        {
            ThrusterBoost();
        }
        else
        {
            AdjustCamera(normalSpeedFOV);
            currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
            transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
        }

        if(Input.GetAxisRaw("Vertical") <= 0.5f)
        {
            AdjustCamera(startFOV);
        }

        if (Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused && !IsThrusting())
        {
            weapon.ShootMissile();
        }

        if (!IsThrusting())
        {
            StartCoroutine(RechargeThrusters());
        } else
        {
            StopCoroutine(RechargeThrusters());
        }

        thrusters.SetThruster(thrusterStock);
    }

    private void AdjustCamera(float target)
    {
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, target, camCurve.Evaluate(fovT * Time.deltaTime));
    }

    private IEnumerator RechargeThrusters()
    {
        yield return new WaitForSeconds(rechargeDelay);

        thrusterStock = Mathf.MoveTowards(thrusterStock, maxThrusterStock, (IncreaseThrusterAmount * Time.deltaTime));
    }

    private bool IsThrusting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && thrusterStock > 0f && currentForwardSpeed >= (forwardSpeed -5))
        { 
            return true;
        }
        else 
        {
            return false;
        }
    }

    private void ThrusterBoost()
    {
        AdjustCamera(boostSpeedFOV);

        thrusterStock = Mathf.MoveTowards(thrusterStock, 0, (decreaseThrusterAmount * Time.deltaTime));

        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * thrusterSpeed, forwardAcceleration * Time.deltaTime);
        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
    }

    private void RollShip()
    {
        roll = Mathf.Lerp(roll, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * 100f * Time.deltaTime, mouseDistance.x * 100f * Time.deltaTime, roll * rollSpeed * Time.deltaTime, Space.Self);
    }

    //void Shoot()
    //{
    //    Debug.Log("Shooting");
        
    //    RaycastHit hit;
    //    if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
    //    {
    //        Debug.Log(hit.transform.name);
    //    }

    //    Rigidbody clone;

    //    foreach (Transform origin in pointOfOrigin)
    //    {
    //        clone = Instantiate(projectile, origin.position, origin.rotation);
    //        clone.velocity = origin.transform.TransformDirection(Vector3.forward * projectileSpeed);
    //    }
    //}
}