using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHealth : MonoBehaviour
{ 
    public void PlayerDie()
    {
        Debug.Log("Player Died!");

        Camera cam;
        cam = Camera.main;

        cam.transform.parent = null;

        Destroy(gameObject);

        InGameMenu.instance.DisplayDeathMenu();
    }
}
