using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public bool PlayerIsDead = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;

    public bool GameState;

    [SerializeField] private string restartScene;
    [SerializeField] private string mainMenuScene;

    private HiveManager hiveManager;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
        deathMenuUI.SetActive(false); 
    }

    private void Update()
    {
        GameState = GameIsPaused;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

        if (PlayerIsDead)
            DisplayDeathMenu();
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void RestartGame()
    {
        Resume();

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(restartScene);
    }

    public void DisplayDeathMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GameIsPaused = true;

        deathMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
