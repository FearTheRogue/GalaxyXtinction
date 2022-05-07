using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=76WOa6IU_s8
/// 
/// Modifications to this script was adding a fps toggle, and a sorting list method. Plans are 
/// to implement a loading screen.
/// 
/// </summary>

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] private Text resolutionText;

    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Text masterText, musicText, sfxText;
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;

    [SerializeField] private Toggle fpsToggle;

    //[SerializeField] private static Display[] displays;
    //[SerializeField] private List<Display> dis = new List<Display>();
    //[SerializeField] private Dropdown displaysDropdown;
    //[SerializeField] private Text numdisplays; 

    private void Start()
    {
        // Assigns the Screen.fullScreen 
        fullscreenToggle.isOn = Screen.fullScreen;
        
        // The current state of the fps counter is assigned
        fpsToggle.isOn = FPSCounter.instance.isDisplayed;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        // 
        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;

                UpdateResolutionText();

                resolutions.Sort(SortByRes);
                resolutions.Reverse();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            
            selectedResolution = resolutions.Count - 1;

            UpdateResolutionText();

            resolutions.Sort(SortByRes);
            resolutions.Reverse();
        }

        //List<DisplayInfo> displayInfos = new List<DisplayInfo>();

        //for (int i = 0; i < Display.displays.Length; i++)
        //{
        //    Debug.Log(Display.displays[i]);
        //    dis.Add(Display.displays[i]);
        //    displayInfos.Add(Display.displays[i]);
        //}

        //numdisplays.text = displayInfos.Count.ToString();

        // Retrieves the value from the mixer groups and passes out to float volume
        float volume = 0;
        mixer.GetFloat("MasterVol", out volume);
        masterSlider.value = volume;
        mixer.GetFloat("MusicVol", out volume);
        musicSlider.value = volume;
        mixer.GetFloat("SFXVol", out volume);
        sfxSlider.value = volume;

        // Update UI
        // 80 is added to each value, due to how Unity audio works 
        masterText.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicText.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxText.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    // Sorts the repolution list by the horizontal value
    static int SortByRes(ResItem item1, ResItem item2)
    {
        return item1.horizontal.CompareTo(item2.horizontal);
    }

    // Cycles the resolutions list
    public void ResLeft()
    {
        selectedResolution--;

        if (selectedResolution < 0)
            selectedResolution = 0;

        UpdateResolutionText();
    }

    // Cycles the resolutions list
    public void ResRight()
    {
        selectedResolution++;

        if (selectedResolution > resolutions.Count - 1)
            selectedResolution = resolutions.Count - 1;

        UpdateResolutionText();
    }

    // Updates the UI text
    public void UpdateResolutionText()
    {
        resolutionText.text = resolutions[selectedResolution].horizontal.ToString() + "x" + resolutions[selectedResolution].vertical.ToString();
    }

    // Apply graphical changes
    public void ApplyGraphics() 
    {
        if (vsyncToggle.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        FPSCounter.instance.isDisplayed = fpsToggle.isOn;
        FPSCounter.instance.SaveDisplay();

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    } 

    // The slider value is used to set and store in the audio mixer and PlayerPrefs, respectfully
    public void SetMasterVolume()
    {
        masterText.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();

        mixer.SetFloat("MasterVol", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    // The slider value is used to set and store in the audio mixer and PlayerPrefs, respectfully
    public void SetMusicVolume()
    {
        musicText.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        mixer.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    // The slider value is used to set and store in the audio mixer and PlayerPrefs, respectfully
    public void SetSFXVolume()
    {
        sfxText.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

        mixer.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }
}

// Custom Resolution class
[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}