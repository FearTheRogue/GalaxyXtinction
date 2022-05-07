using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// This script had the most iterations to it, and a number of tutorials was used to help
/// implement player teleportation. 
/// 
/// Tutorial Video: https://www.youtube.com/watch?v=lQ3OT9xp_5A&t=904s
/// This video was the most helpful. However appears noticably different
/// 
/// 
/// Handles the overall player warping to and from planets.
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private int currentSceneIndex;

    // Implement at a later date
    //[SerializeField] private GameObject loadingScreen;
    //[SerializeField] private Slider loadingBar;
    //[SerializeField] private Text loadingText;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] warpPortals;
    private WarpHandler portal;
    [SerializeField] private int currentWarpNumber;

    Coroutine loadingSceneInProgress;

    private HiveManager hiveManager;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        warpPortals = GameObject.FindGameObjectsWithTag("Warp");

        // Loading screen
        /*
        if (loadingScreen == null)
            loadingScreen = InGameMenu.instance.GetLoadingPanel();

        loadingScreen.SetActive(true);

        if (loadingBar == null)
            loadingBar = loadingScreen.GetComponentInChildren<Slider>();
        if (loadingText == null)
            loadingText = loadingScreen.GetComponentInChildren<Text>();

        loadingScreen.SetActive(false);
        */

        if (portal == null)
        {
            return;
        }
    }

    // Final method that is called during scene loading
    private void OnLevelFinishedLoading(int comingFromSceneIndex)
    {
        Debug.Log("Scene has loaded");

        FPSCounter.instance.ResetMinAndMax();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        warpPortals = GameObject.FindGameObjectsWithTag("Warp");

        // Compares each warpPortals with the one from the previous scene
        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i].GetComponent<WarpHandler>().sceneToLoadIndex == comingFromSceneIndex)
            {
                portal = warpPortals[i].GetComponent<WarpHandler>();

                player = GameObject.FindGameObjectWithTag("Player");

                // If the player is now in space
                if(player.transform.parent != null)
                {
                    player = player.transform.root.gameObject;

                    hiveManager = GameObject.Find("Hive Manager").GetComponent<HiveManager>();
                    hiveManager.GetHiveShipSpawner();

                    CursorManager.instance.ActivateCrosshairCursor();

                    Debug.Log("Loading into space");
                }

                // If the player is on planet, deactivate the CharacterController component
                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = false;

                    CursorManager.instance.ActivateCenter();
                }

                // Sets the player position to the spawn point of the correct warp point
                player.transform.position = portal.GetSpawnPoint().position;
                player.transform.rotation = portal.GetSpawnPoint().rotation;

                // If the player is on planet, reactivate the CharacterController component
                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = true;
                }

                Debug.Log("Portal: " + warpPortals[i].name);

                return;
            }
        }
    }

    // Entry point to transitioning between space and planets, and vise versa
    public void StartToLoadLevel(int levelToLoad)
    {
        //loadingScreen.SetActive(true);

        if (levelToLoad != 1)
        {
            hiveManager = GameObject.Find("Hive Manager").GetComponent<HiveManager>();
            hiveManager.SetHiveShipSpawner();
        }

        if (loadingSceneInProgress != null)
        {
            return;
        }

        loadingSceneInProgress = StartCoroutine(LoadSceneAsync(levelToLoad));
    }

    // LoadSceneAsync to the desired scene
    private IEnumerator LoadSceneAsync(int levelToLoad)
    {
        int comingFromSceneIndex;
        comingFromSceneIndex = SceneManager.GetActiveScene().buildIndex;

        yield return SceneManager.LoadSceneAsync(levelToLoad);

        OnLevelFinishedLoading(comingFromSceneIndex);

        loadingSceneInProgress = null;  
    }

    // Handles transitioning from main menu to space scene
    public void SceneToLoad(string sceneToBeLoaded)
    {
       // loadingScreen.SetActive(true);

        InGameMenu.GameIsPaused = false;

        FPSCounter.instance.ResetMinAndMax();

        SceneManager.LoadScene(sceneToBeLoaded);

        //StartCoroutine(LoadLoad(sceneToBeLoaded));
    }

    // Sets the right cursor
    public void CheckCursor()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            CursorManager.instance.ActivateCenter();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
