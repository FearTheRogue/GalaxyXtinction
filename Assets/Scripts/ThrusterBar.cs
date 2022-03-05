using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetThruster(float value)
    {
        slider.value = value;
    }

    public void SetMaxThrusterValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }
}
