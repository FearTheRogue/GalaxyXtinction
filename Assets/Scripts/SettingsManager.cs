using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] private Text resolutionText;

    [SerializeField] private List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

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
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}