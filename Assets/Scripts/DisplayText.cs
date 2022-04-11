using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hintPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hintPanel.SetActive(false);
    }
}
