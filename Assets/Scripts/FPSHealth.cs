using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHealth : MonoBehaviour
{
    [SerializeField] private PauseMenu menu;

    private void Awake()
    {
        menu = GameObject.Find("Menu Manager").gameObject.GetComponent<PauseMenu>();
       
    }

    public void PlayerDie()
    {
        Debug.Log("Player Died!");

        Camera cam;
        cam = Camera.main;

        cam.transform.parent = null;

        Destroy(gameObject);

        menu.DisplayDeathMenu();
    }
}
