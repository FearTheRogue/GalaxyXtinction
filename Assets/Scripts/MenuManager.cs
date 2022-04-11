using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button playBtn, closeTestingBtn, okayBtn, quitBtn;
    [SerializeField] GameObject closeTestingPopup, aboutGamePopup;
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

    public void CloseAboutGame()
    {
        aboutGamePopup.gameObject.SetActive(false);
    }

    public void CloseTestingPopUp()
    {
        closeTestingPopup.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
