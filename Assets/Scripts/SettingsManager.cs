using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] private Text resolutionText;

    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Text masterText, musicText, sfxText;
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;

    //[SerializeField] private static Display[] displays;
    //[SerializeField] private List<Display> dis = new List<Display>();
    //[SerializeField] private Dropdown displaysDropdown;
    //[SerializeField] private Text numdisplays; 

    private void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

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

        float volume = 0;
        mixer.GetFloat("MasterVol", out volume);
        masterSlider.value = volume;
        mixer.GetFloat("MusicVol", out volume);
        musicSlider.value = volume;
        mixer.GetFloat("SFXVol", out volume);
        sfxSlider.value = volume;

        masterText.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicText.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxText.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    static int SortByRes(ResItem item1, ResItem item2)
    {
        return item1.horizontal.CompareTo(item2.horizontal);
    }

    public void ResLeft()
    {
        selectedResolution--;

        if (selectedResolution < 0)
            selectedResolution = 0;

        UpdateResolutionText();
    }

    public void ResRight()
    {
        selectedResolution++;

        if (selectedResolution > resolutions.Count - 1)
            selectedResolution = resolutions.Count - 1;

        UpdateResolutionText();
    }

    public void UpdateResolutionText()
    {
        resolutionText.text = resolutions[selectedResolution].horizontal.ToString() + "x" + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics() 
    {
        if (vsyncToggle.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    } 

    public void SetMasterVolume()
    {
        masterText.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();

        mixer.SetFloat("MasterVol", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        musicText.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        mixer.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        sfxText.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

        mixer.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}