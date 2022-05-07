using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Handles the Menus in the Main menu scene
/// 
/// </summary>

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject closeTestingPopup, aboutGamePopup, settingsPopUp;
    [SerializeField] string levelToLoad;

    private void Start()
    {
        Time.timeScale = 1f;

        CursorManager.instance.ActivateNormalCursor();
    }

    public void PlayGame()
    {
        // Play the specified audio clip
        AudioManager.instance.Play("UI Selected");
        
        // Sets the correct cursor
        CursorManager.instance.ActivateCrosshairCursor();

        // Delete all the values stored in the PlayerPrefs
        PlayerPrefs.DeleteAll();
        
        // Loads the scene
        GameManager.instance.SceneToLoad(levelToLoad);
    }

    // About game option
    public void AboutGame()
    {
        AudioManager.instance.Play("UI Selected");

        aboutGamePopup.gameObject.SetActive(true);
    }

    // Close any option that is open
    public void CloseWindow()
    {
        AudioManager.instance.Play("UI Selected");

        if (aboutGamePopup.activeInHierarchy)
            aboutGamePopup.gameObject.SetActive(false);

        if(closeTestingPopup)
            closeTestingPopup.gameObject.SetActive(false);
    
        if(settingsPopUp)
            settingsPopUp.gameObject.SetActive(false);
    }

    // Open the settings menu
    public void OpenSettingsMenu()
    {
        AudioManager.instance.Play("UI Selected");

        settingsPopUp.gameObject.SetActive(true);
    }

    // Quit the application
    public void QuitGame()
    {
        AudioManager.instance.Play("UI Selected");

        Application.Quit();
    }
}
