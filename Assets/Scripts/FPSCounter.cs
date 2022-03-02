using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsDisplay;
    [SerializeField] private TMP_Text avgFPSDisplay;
    int framesPassed = 0;
    float fpsTotal = 0f;

    private float[] frameDeltaTimeArray;

    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }

    private void Update()
    {
        frameDeltaTimeArray[framesPassed] = Time.unscaledDeltaTime;
        framesPassed = (framesPassed + 1) % frameDeltaTimeArray.Length;

        fpsDisplay.text = "FPS: " + Mathf.RoundToInt(CalculateFPS()).ToString();

        //fpsTotal += fps;
        //framesPassed++;
        //avgFPSDisplay.text = "AVG FPS: " + (fpsTotal / framesPassed);
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray) 
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}
