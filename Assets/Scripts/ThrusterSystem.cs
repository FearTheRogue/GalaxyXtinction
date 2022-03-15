using System.Collections;
using UnityEngine;

public class ThrusterSystem : MonoBehaviour
{
    private ShipController shipController;
    private CameraMovement shipCamera;

    [Header("Thruster Speed Settings")]
    [SerializeField] private ThrusterBar thrusters;
    [SerializeField] private float thrusterSpeed;
    [SerializeField] private float thrusterStock;
    [SerializeField] private float maxThrusterStock;
    [SerializeField] private float decreaseThrusterAmount;
    [SerializeField] private float IncreaseThrusterAmount;
    [SerializeField] private float rechargeDelay;
    [SerializeField] private bool isThrusting;
    [SerializeField] private bool canThrust;

    private void Start()
    {
        shipController = GetComponent<ShipController>();
        shipCamera = GetComponent<CameraMovement>();

        thrusterStock = maxThrusterStock;
        thrusters.SetMaxThrusterValue(thrusterStock);
    }

    private void Update()
    {
        isThrusting = CheckCanThrust();

        if (!CheckCanThrust() && thrusterStock < maxThrusterStock)
        {
            StartCoroutine(RechargeThrusters());
        }
        else
        {
            StopCoroutine(RechargeThrusters());
            canThrust = true;
        }

        thrusters.SetThruster(thrusterStock);
    }

    private void LateUpdate()
    {
        if (CheckCanThrust())
        {
            ThrusterBoost();
        }
    }

    private IEnumerator RechargeThrusters()
    {
        canThrust = false;

        yield return new WaitForSeconds(rechargeDelay);

        thrusterStock = Mathf.MoveTowards(thrusterStock, maxThrusterStock, (IncreaseThrusterAmount * Time.deltaTime));

        if (thrusterStock == maxThrusterStock)
            canThrust = true;
    }

    public bool CheckCanThrust()
    {
        if (Input.GetKey(KeyCode.LeftShift) && thrusterStock > 0f && shipController.GetCurrentSpeed() >= 95 && canThrust)
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
        shipCamera.AdjustCamera(shipCamera.boostSpeedFOV);

        thrusterStock = Mathf.MoveTowards(thrusterStock, 0, (decreaseThrusterAmount * Time.deltaTime));
    }


    public float GetThrusterSpeed()
    {
        return thrusterSpeed;
    }
}
