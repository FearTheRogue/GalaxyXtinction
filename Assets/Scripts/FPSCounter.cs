using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    public static FPSCounter instance;
    public bool isDisplayed = true;

    private int frameCounter = 0;
    private float timeCounter = 0.0f;
    private float refreshTime = 0.1f;

    private float minFramerate = 1000f;
    private float maxFramerate = 0;

    [SerializeField] private TMP_Text framerateText;
    [SerializeField] private TMP_Text minFramerateText;
    [SerializeField] private TMP_Text maxFramerateText;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!PlayerPrefs.HasKey("IsFPSDisplayed"))
            isDisplayed = true;
        else
            isDisplayed = PlayerPrefs.GetInt("IsFPSDisplayed") != 0;
    }

    private void Start()
    {
        StartCoroutine(ResetMinFramerate());
    }

    public void ResetMinAndMax()
    {
        StartCoroutine(ResetMinFramerate());
    }

    private IEnumerator ResetMinFramerate()
    {
        yield return new WaitForSeconds(1.0f);

        minFramerateText.text = "";
        maxFramerateText.text = "";
        minFramerate = 1000f;
        maxFramerate = 0;
    }

    private void Update()
    {
        if (timeCounter < refreshTime)
        {
            timeCounter += Time.deltaTime;
            frameCounter++;
        } 
        else
        {
            float lastFramerate = frameCounter / timeCounter;

            if (minFramerate > lastFramerate)
                minFramerate = lastFramerate;

            if (maxFramerate < lastFramerate)
                maxFramerate = lastFramerate;

            frameCounter = 0;
            timeCounter = 0.0f;

            if (isDisplayed)
            {
                framerateText.text = "FPS: " + lastFramerate.ToString("n2");
                minFramerateText.text = "Min: " + minFramerate.ToString("n2");
                maxFramerateText.text = "Max: " + maxFramerate.ToString("n2");
            } 
            else
            {
                framerateText.text = "";
                minFramerateText.text = "";
                maxFramerateText.text = "";
            }
        }
    }


    public void SaveDisplay()
    {
        PlayerPrefs.SetInt("IsFPSDisplayed", (isDisplayed ? 1 : 0));
    }

    //public static FPSCounter instance;

    //[SerializeField] private TMP_Text fpsDisplay;
    //[SerializeField] private TMP_Text avgFPSDisplay;
    //int framesPassed = 0;
    //float avgFps = 0;
    //float fpsTotal = 0f;

    //private float[] frameDeltaTimeArray;

    //public float displayValue;

    //public bool isDisplayed;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        DontDestroyOnLoad(gameObject);
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }

    //    isDisplayed = PlayerPrefs.GetInt("IsFPSDisplayed") != 0;

    //    frameDeltaTimeArray = new float[50];
    //}

    //private void Update()
    //{
    //    if (isDisplayed)
    //    {
    //        frameDeltaTimeArray[framesPassed] = Time.unscaledDeltaTime;
    //        framesPassed = (framesPassed + 1) % frameDeltaTimeArray.Length;

    //        fpsDisplay.text = "FPS: " + Mathf.RoundToInt(CalculateFPS()).ToString();

    //        avgFps += ((Time.deltaTime / Time.timeScale) - avgFps) * 0.03f;
    //        avgFPSDisplay.text = "AVG: " + Mathf.RoundToInt((1f / avgFps)).ToString();
    //    }
    //    else
    //    {
    //        fpsDisplay.text = "";
    //        avgFPSDisplay.text = "";
    //    }
    //    //fpsTotal += fps;
    //    //framesPassed++;
    //    //avgFPSDisplay.text = "AVG FPS: " + (fpsTotal / framesPassed);
    //}

    //private float CalculateFPS()
    //{
    //    float total = 0f;
    //    foreach (float deltaTime in frameDeltaTimeArray) 
    //    {
    //        total += deltaTime;
    //    }

    //    return frameDeltaTimeArray.Length / total;
    //}

    //public void SaveDisplay()
    //{
    //    PlayerPrefs.SetInt("IsFPSDisplayed", (isDisplayed ? 1 : 0));
    //}
}
