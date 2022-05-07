using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=_QajrabyTJc&t=774s
/// This script was implemented from the tutorial along with the 'MouseLook' script.
/// 
/// Not much modification was made to this script. Apart from adding the option to 
/// allow the player to "sprint".
/// 
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [SerializeField] private float sprintSpeed;
    [SerializeField] private bool isSprinting;

    Vector3 velocity;
    public bool isGrounded;

    void Update()
    {
        // Draws a sphere and checks if it is colliding with the groundMask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Player can Jump if they are grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Player can sprint if they are grounded
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            // moving at sprint speed
            controller.Move(move * sprintSpeed * Time.deltaTime);
            isSprinting = true;
        }
        else
        {
            // moving at normal speed
            controller.Move(move * speed * Time.deltaTime);
            isSprinting = false;
        }

        // Apply the gravity value
        velocity.y += gravity * Time.deltaTime;

        // move player using the velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
