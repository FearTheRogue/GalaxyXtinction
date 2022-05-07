using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=J6QR4KzNeJU
/// 
/// Handles the player ship controls.
/// 
/// Modifications to this script include Thrusting mechanic and shooting.
/// 
/// </summary>

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

    [Header("Roll Settings")]
    private float roll;
    [SerializeField] private float rollSpeed = 90f;
    [SerializeField] private float rollAcceleration = 5f;

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
    }

    void Update()
    {
        if (!canRotate)
        {
            // Calculates the centre of the screen
            mouseLookInput.x = Input.mousePosition.x;
            mouseLookInput.y = Input.mousePosition.y;

            mouseDistance.x = (mouseLookInput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (mouseLookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        }

        RollShip();

        if (CheckShoot())
        {
            // Shooting projectiles
            weapon.ShootMissile();
        }

        if (thrusterSystem.isThrusting)
        {
            // currentForwardSpeed is set to the thruster speed
            currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * thrusterSystem.GetThrusterSpeed(), forwardAcceleration * Time.deltaTime);
        }
        else
        {
            // currentForwardSpeed is set to the normal speed
            currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);

            // If the currentForwardSpeed exceeds a certain value
            if (currentForwardSpeed <= 50)
            {
                // Adjust the camera to the start FOV
                shipCamera.AdjustCamera(shipCamera.startFOV);

                // Stops audio, if playing
                if (AudioManager.instance.IsClipPlaying("Engine Start"))
                    AudioManager.instance.Stop("Engine Start");

                hasEngineStartPlayed = false;

                // Stops audio, if playing
                if (AudioManager.instance.IsClipPlaying("Regular Boost"))
                    AudioManager.instance.Stop("Regular Boost");
            }
            else
            { 
                // Adjust the camera to the normal FOV
                shipCamera.AdjustCamera(shipCamera.normalSpeedFOV);

                // If the audio isnt playing
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
        if(Input.GetMouseButton(0) && !InGameMenu.GameIsPaused && currentForwardSpeed <= 150)
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
}