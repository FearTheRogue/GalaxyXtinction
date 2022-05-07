using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Handles the health slider UI
/// 
/// </summary>

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    // Sets the value of the health 
    public void SetHealth(int health)
    {
        slider.value = health;

        // Updates UI
        if(text != null)
        text.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    }

    // Sets the value of the max health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        if(text != null)
        text.text = health.ToString() + "/" + slider.maxValue.ToString();
    }
}
