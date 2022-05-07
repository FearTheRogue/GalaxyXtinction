using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Handles the player death on planets.
/// 
/// </summary>

public class FPSHealth : MonoBehaviour
{ 
    public void PlayerDie()
    {
        Debug.Log("Player Died!");

        Camera cam;
        cam = Camera.main;

        // Get the main camera and isolates the gameobject
        cam.transform.parent = null;

        Destroy(gameObject);

        // Displays the death menu
        InGameMenu.instance.DisplayDeathMenu();
    }
}
