using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Handles the thrusting mechanic, used by the player.
/// 
/// </summary>

public class ThrusterSystem : MonoBehaviour
{
    private ShipController shipController;
    private CameraMovement shipCamera;

    [SerializeField] private ParticleSystem thrusterFX;

    [Header("Thruster Speed Settings")]
    [SerializeField] private ThrusterBar thrusters;
    [SerializeField] private float thrusterSpeed;
    [SerializeField] private float thrusterStock;
    [SerializeField] private float maxThrusterStock;
    [SerializeField] private float decreaseThrusterAmount;
    [SerializeField] private float IncreaseThrusterAmount;
    [SerializeField] private float rechargeDelay;
    [SerializeField] public bool isThrusting;
    [SerializeField] public bool canThrust;

    [SerializeField] private bool unlimitedBoost = false;

    private bool hasInitialSFXPlayed = false;

    [SerializeField] private float currentTime = 0;
    [SerializeField] private float maxTime;

    private void Awake()
    {
        thrusterFX.Stop();
    }

    private void Start()
    {
        shipController = GetComponent<ShipController>();
        shipCamera = GetComponent<CameraMovement>();

        thrusterStock = maxThrusterStock;
        thrusters.SetMaxThrusterValue(thrusterStock);
    }

    private void Update()
    {
        // Multiple checks if thrusting
        if (Input.GetKey(KeyCode.LeftShift) && thrusterStock > 0f && shipController.GetCurrentSpeed() >= 95 && canThrust)
        {
            ThrusterBoost();
            isThrusting = true;
        }
        else
        {
            // If not thrusting, adjust camera with the normal FOV
            shipCamera.AdjustCamera(shipCamera.normalSpeedFOV);
            isThrusting = false;
            canThrust = true;
        }

        // If the thrusterStocks value is less than 0.5, canThrust is false
        if(thrusterStock <= 0.5)
        {
            canThrust = false;
        }
        else if (thrusterStock < maxThrusterStock && !isThrusting && canThrust)
        {
            // If not thrusting, RechargeThrusters
            RechargeThrusters();
        }

        // Adds a delay to thrust, before being able to RechargeThrusters
        if (!canThrust)
        {
            currentTime += Time.deltaTime;

            thrusterFX.Stop();

            if (currentTime >= maxTime)
            {
                RechargeThrusters();
                currentTime = 0;
            }
        }
    }

    private void RechargeThrusters()
    {
        hasInitialSFXPlayed = false;

        if (AudioManager.instance.IsClipPlaying("Thruster Boost"))
            AudioManager.instance.Stop("Thruster Boost");

        thrusterFX.Stop();

        // thrusterStock value is increased by the thruster amount
        thrusterStock = Mathf.MoveTowards(thrusterStock, maxThrusterStock, (IncreaseThrusterAmount * Time.deltaTime));

        // Updates the UI
        thrusters.SetThruster(thrusterStock);

        // If thruster amount is full, canThrust is true
        if (thrusterStock == maxThrusterStock)
            canThrust = true;
    }

    private void ThrusterBoost()
    {
        // adjusts the camera to boost FOV
        shipCamera.AdjustCamera(shipCamera.boostSpeedFOV);

        // Plays the effect
        thrusterFX.Play();

        if (!hasInitialSFXPlayed)
        {
            if (!AudioManager.instance.IsClipPlaying("Initial Boost"))
                AudioManager.instance.Play("Initial Boost");

            hasInitialSFXPlayed = true;
        }

        if (!AudioManager.instance.IsClipPlaying("Thruster Boost"))
            AudioManager.instance.Play("Thruster Boost");

        if(!unlimitedBoost)
            thrusterStock = Mathf.MoveTowards(thrusterStock, 0, (decreaseThrusterAmount * Time.deltaTime));
        
        thrusters.SetThruster(thrusterStock);
    }

    public float GetThrusterSpeed()
    {
        return thrusterSpeed;
    }
}
