using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Handles the thruster slider UI
/// 
/// </summary>

public class ThrusterBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    // Sets the value of the thruster 
    public void SetThruster(float value)
    {
        slider.value = value;
        text.text = Mathf.RoundToInt(value).ToString() + "/ " + slider.maxValue.ToString();
    }

    // Sets the value of the max thruster 
    public void SetMaxThrusterValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;

        text.text = Mathf.RoundToInt(value).ToString() + "/ " + slider.maxValue.ToString();
    }
}
