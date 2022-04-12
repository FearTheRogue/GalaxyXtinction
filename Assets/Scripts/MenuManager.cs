using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button playBtn, closeTestingBtn, okayBtn, settingsBtn,quitBtn;
    [SerializeField] GameObject closeTestingPopup, aboutGamePopup, settingsPopUp;
    [SerializeField] string levelToLoad;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PlayGame()
    {
        AudioManager.instance.Play("UI Selected");

        PlayerPrefs.DeleteAll();
        GameManager.instance.SceneToLoad(levelToLoad);
    }

    public void AboutGame()
    {
        AudioManager.instance.Play("UI Selected");

        aboutGamePopup.gameObject.SetActive(true);
    }

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

    public void OpenSettingsMenu()
    {
        AudioManager.instance.Play("UI Selected");

        settingsPopUp.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        AudioManager.instance.Play("UI Selected");

        Application.Quit();
    }
}
