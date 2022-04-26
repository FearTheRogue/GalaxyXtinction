using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu instance;

    public static bool GameIsPaused = false;
    public bool PlayerIsDead = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public GameObject loadingScreen;

    public bool GameState;

    [SerializeField] private string restartScene;
    [SerializeField] private string mainMenuScene;

    private HiveManager hiveManager;

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
    }

    private void Update()
    {
        GameState = GameIsPaused;
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

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
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            CursorManager.instance.ActivateCrosshairCursor();
        } 
        else
        {
            CursorManager.instance.ActivateCenter();
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        CursorManager.instance.ActivateNormalCursor();

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void RestartGame()
    {
        CursorManager.instance.ActivateCrosshairCursor();

        PlayerIsDead = false;
        GameIsPaused = false;

        if (deathMenuUI.activeInHierarchy)
            deathMenuUI.SetActive(false);

        if (pauseMenuUI.activeInHierarchy)
            pauseMenuUI.SetActive(false);

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(restartScene);
    }

    public void DisplayDeathMenu()
    {
        CursorManager.instance.ActivateNormalCursor();

        GameIsPaused = true;

        if (AudioManager.instance.IsClipPlaying("Warning Sound"))
            AudioManager.instance.Stop("Warning Sound");

        deathMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        GameIsPaused = false;
        PlayerIsDead = false;

        if (deathMenuUI.activeInHierarchy)
            deathMenuUI.SetActive(false);

        if (pauseMenuUI.activeInHierarchy)
            pauseMenuUI.SetActive(false);

        SceneManager.LoadScene(mainMenuScene);
    }

    public GameObject GetLoadingPanel()
    {
        return loadingScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
