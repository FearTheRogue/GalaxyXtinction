using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=_QajrabyTJc&t=774s
/// This script was implemented from the tutorial along with the 'PlayerMovement' script.
/// 
/// Not much modification required to this script. Apart from the inclusion of
/// when the game is paused, dont execute the code in the Update() method.
/// 
/// </summary>

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    void Update()
    {
        // If game is not paused
        if (!InGameMenu.GameIsPaused)
        {
            // Mouse X Axis
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            // Mouse Y Axis
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Decreasing xRotation with mouseY
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
