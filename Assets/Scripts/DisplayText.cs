using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// Handles the activation/ deactivation of the hint panel, used on planets.
/// 
/// </summary>

public class DisplayText : MonoBehaviour
{
    [SerializeField] private GameObject hintPanel;

    private void Start()
    {
        if(hintPanel == null)
        {
            hintPanel = GameObject.Find("Hint Panel");
        }

        hintPanel.SetActive(false);
    }

    // When triggered the hint panel is set active
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hintPanel.SetActive(true);
        }
    }

    // When triggered the hint panel is set to deactive
    private void OnTriggerExit(Collider other)
    {
        hintPanel.SetActive(false);
    }
}
