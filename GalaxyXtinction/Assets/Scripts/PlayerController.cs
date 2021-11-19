using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed;
    public float currentForwardSpeed;
    public float forwardAcceleration;

    public float roll;
    public float rollSpeed = 90f;
    public float rollAcceleration = 5f;

    public Vector2 mouseLookInput, screenCenter, mouseDistance;

    // Start is called before the first frame update
    void Start()
    {
        // Finding the screen center
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
    }

    // Update is called once per frame
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
    }
}