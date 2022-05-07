using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=RTfMWE-NDTE
/// 
/// Only small modifations to this script has been made. Such as the 'ResetMinAndMax()' method.
/// Other additions were implemented, like saves if the display is enabled or disabled from
/// the 'SettingsManager' script.
/// 
/// </summary>

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

        // Check if the save value exists 
        if (!PlayerPrefs.HasKey("IsFPSDisplayed"))
            isDisplayed = true;
        else
            // if it doesn't exist
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

    // Resets Min and Max frame rate values and text components
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

    // Saves the current state of the fps display
    public void SaveDisplay()
    {
        PlayerPrefs.SetInt("IsFPSDisplayed", (isDisplayed ? 1 : 0));
    }
}
