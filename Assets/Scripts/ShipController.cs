using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private CameraMovement shipCamera;
    private ThrusterSystem thrusterSystem;

    [SerializeField] private bool canRotate = true;

    [Header("Ship Settings")]
    [SerializeField] private Transform[] pointOfOrigin;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Weapon weapon;

    [Header("Normal Speed Settings")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float forwardAcceleration;

    private float roll;
    private float rollSpeed = 90f;
    private float rollAcceleration = 5f;

    private Vector2 mouseLookInput, screenCenter, mouseDistance;
    public Rigidbody Rb => rb;

    private bool hasEngineStartPlayed = false;

    private void Awake()
    {
        shipCamera = GetComponent<CameraMovement>();
        thrusterSystem = GetComponent<ThrusterSystem>();

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Finding the screen center
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        //Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (!canRotate)
        {
            mouseLookInput.x = Input.mousePosition.x;
            mouseLookInput.y = Input.mousePosition.y;

            mouseDistance.x = (mouseLookInput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (mouseLookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        }

        RollShip();

        if (CheckShoot())
        {
            weapon.ShootMissile();
        }

        if (thrusterSystem.CheckCanThrust())
        {
            currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * thrusterSystem.GetThrusterSpeed(), forwardAcceleration * Time.deltaTime);
        }
        else
        {
            currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);

            if (currentForwardSpeed <= 50)
            {
                shipCamera.AdjustCamera(shipCamera.startFOV);

                if (AudioManager.instance.IsClipPlaying("Engine Start"))
                    AudioManager.instance.Stop("Engine Start");

                hasEngineStartPlayed = false;

                if (AudioManager.instance.IsClipPlaying("Regular Boost"))
                    AudioManager.instance.Stop("Regular Boost");
            }
            else
            { 
                shipCamera.AdjustCamera(shipCamera.normalSpeedFOV);

                if (!hasEngineStartPlayed)
                {
                    if (!AudioManager.instance.IsClipPlaying("Engine Start"))
                        AudioManager.instance.Play("Engine Start");

                    hasEngineStartPlayed = true;
                }

                if (!AudioManager.instance.IsClipPlaying("Regular Boost"))
                    AudioManager.instance.Play("Regular Boost");
            }
        }

        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
    }

    private bool CheckShoot()
    {
        if(Input.GetMouseButtonDown(0) && !InGameMenu.GameIsPaused && currentForwardSpeed <= 150)
        {
            return true;
        }

        return false;
    }

    private void RollShip()
    {
        roll = Mathf.Lerp(roll, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * 100f * Time.deltaTime, mouseDistance.x * 100f * Time.deltaTime, roll * rollSpeed * Time.deltaTime, Space.Self);
    }

    public float GetCurrentSpeed()
    {
        return currentForwardSpeed;
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