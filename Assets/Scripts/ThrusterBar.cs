using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    public void SetThruster(float value)
    {
        slider.value = value;
        text.text = Mathf.RoundToInt(value).ToString() + "/ " + slider.maxValue.ToString();
    }

    public void SetMaxThrusterValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;

        text.text = Mathf.RoundToInt(value).ToString() + "/ " + slider.maxValue.ToString();
    }
}
