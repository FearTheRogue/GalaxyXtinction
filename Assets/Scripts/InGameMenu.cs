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
    public bool PlayerHasWon = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public GameObject winMenuUI;
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

    private void Start()
    {
        if (deathMenuUI.activeInHierarchy)
            deathMenuUI.SetActive(false);

        if (pauseMenuUI.activeInHierarchy)
            pauseMenuUI.SetActive(false);

        if (winMenuUI.activeInHierarchy)
            winMenuUI.SetActive(false);
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

        if (PlayerHasWon)
            DisplayWinScreenMenu();
        else
            winMenuUI.SetActive(false);
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
        PlayerHasWon = false;
        GameIsPaused = false;

        Time.timeScale = 1f;

        if (deathMenuUI.activeInHierarchy)
            deathMenuUI.SetActive(false);

        if (pauseMenuUI.activeInHierarchy)
            pauseMenuUI.SetActive(false);

        if (winMenuUI.activeInHierarchy)
            winMenuUI.SetActive(false);

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
        winMenuUI.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            hiveManager = GameObject.Find("Hive Manager").GetComponent<HiveManager>();
            hiveManager.ResetHiveShipSpawner();
        }

        GameIsPaused = false;
        PlayerIsDead = false;
        PlayerHasWon = false;

        if (deathMenuUI.activeInHierarchy)
            deathMenuUI.SetActive(false);

        if (pauseMenuUI.activeInHierarchy)
            pauseMenuUI.SetActive(false);

        if (winMenuUI.activeInHierarchy)
            winMenuUI.SetActive(false);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void DisplayWinScreenMenu()
    {
        CursorManager.instance.ActivateNormalCursor();

        GameIsPaused = true;

        Time.timeScale = 0f;

        winMenuUI.SetActive(true);
        deathMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
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
