using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float forwardAcceleration;

    private float roll;
    private float rollSpeed = 90f;
    private float rollAcceleration = 5f;

    private Vector2 mouseLookInput, screenCenter, mouseDistance;

    public Camera cam;
    //public Rigidbody projectile;
    public Transform[] pointOfOrigin;

    //public float projectileSpeed = 100f;

    [SerializeField] private Rigidbody rb;
    public Rigidbody Rb => rb;

    [SerializeField] private Weapon weapon;

    void Start()
    {
        // Finding the screen center
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;

        weapon = GetComponent<Weapon>();
    }

    void Update()
    {
        mouseLookInput.x = Input.mousePosition.x;
        mouseLookInput.y = Input.mousePosition.y;

        mouseDistance.x = (mouseLookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (mouseLookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        roll = Mathf.Lerp(roll, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);
        transform.Rotate(-mouseDistance.y * 100f * Time.deltaTime, mouseDistance.x * 100f * Time.deltaTime, roll * rollSpeed * Time.deltaTime, Space.Self);
        
        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);

        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused)
        {
            weapon.ShootMissile();
        }
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