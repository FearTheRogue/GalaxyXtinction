using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void IsTestVisible()
    {
        Debug.Log("Text Visible");

        gameObject.SetActive(true);
    }
}
