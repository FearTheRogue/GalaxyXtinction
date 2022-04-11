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
        PlayerPrefs.DeleteAll();
        GameManager.instance.SceneToLoad(levelToLoad);
    }

    public void AboutGame()
    {
        aboutGamePopup.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        if(aboutGamePopup.activeInHierarchy)
            aboutGamePopup.gameObject.SetActive(false);

        if(closeTestingPopup)
            closeTestingPopup.gameObject.SetActive(false);
    
        if(settingsPopUp)
            settingsPopUp.gameObject.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        settingsPopUp.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
