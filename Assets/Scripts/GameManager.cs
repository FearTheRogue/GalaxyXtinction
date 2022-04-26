using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private int currentSceneIndex;

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

        //if (loadingScreen == null)
        //    loadingScreen = InGameMenu.instance.GetLoadingPanel();

        //loadingScreen.SetActive(true);

        //if (loadingBar == null)
        //    loadingBar = loadingScreen.GetComponentInChildren<Slider>();
        //if (loadingText == null)
        //    loadingText = loadingScreen.GetComponentInChildren<Text>();

        //loadingScreen.SetActive(false);

        if (portal == null)
        {
            return;
        }
    }

    private void OnLevelFinishedLoading(int comingFromSceneIndex)
    {
        Debug.Log("Scene has loaded");

        FPSCounter.instance.ResetMinAndMax();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        warpPortals = GameObject.FindGameObjectsWithTag("Warp");

        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i].GetComponent<WarpHandler>().sceneToLoadIndex == comingFromSceneIndex)
            {
                portal = warpPortals[i].GetComponent<WarpHandler>();

                player = GameObject.FindGameObjectWithTag("Player");

                if(player.transform.parent != null)
                {
                    player = player.transform.root.gameObject;

                    hiveManager = GameObject.Find("Hive Manager").GetComponent<HiveManager>();
                    hiveManager.GetHiveShipSpawner();

                    CursorManager.instance.ActivateCrosshairCursor();

                    Debug.Log("Loading into space");
                }

                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = false;

                    CursorManager.instance.ActivateCenter();
                }

                player.transform.position = portal.GetSpawnPoint().position;
                player.transform.rotation = portal.GetSpawnPoint().rotation;

                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = true;
                }

                Debug.Log("Portal: " + warpPortals[i].name);

                return;
            }
        }
    }

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

    private IEnumerator LoadSceneAsync(int levelToLoad)
    {
        int comingFromSceneIndex;
        comingFromSceneIndex = SceneManager.GetActiveScene().buildIndex;

        yield return SceneManager.LoadSceneAsync(levelToLoad);

        OnLevelFinishedLoading(comingFromSceneIndex);

        loadingSceneInProgress = null;  
    }

    public void SceneToLoad(string sceneToBeLoaded)
    {
       // loadingScreen.SetActive(true);

        InGameMenu.GameIsPaused = false;

        FPSCounter.instance.ResetMinAndMax();

        SceneManager.LoadScene(sceneToBeLoaded);

        //StartCoroutine(LoadLoad(sceneToBeLoaded));
    }

    //private IEnumerator LoadLoad(string sceneToBeLoaded)
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToBeLoaded);

    //    asyncLoad.allowSceneActivation = false;

    //    while (!asyncLoad.isDone)
    //    {
    //        loadingBar.value = asyncLoad.progress;
    //        loadingText.text = (asyncLoad.progress * 100).ToString() + "%";

    //        if (asyncLoad.progress >= 0.9f && !asyncLoad.allowSceneActivation)
    //        {
    //            if (AudioManager.instance.IsClipPlaying("Thruster Boost"))
    //            {
    //                AudioManager.instance.Stop("Thruster Boost");
    //            }


    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                asyncLoad.allowSceneActivation = true;
    //            }
    //        }

    //        yield return null;
    //    }
    //}

    public void CheckCursor()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            CursorManager.instance.ActivateCenter();
        }
    }

    public void ActivateLoadingScene()
    {
        //loadingScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //public int SceneToLoad(int sceneNumberToLoad)
    //{
    //    currentWarpNumber = sceneNumberToLoad;
        
    //    if (SceneManager.GetActiveScene().buildIndex == 0)
    //    {
    //        //SceneManager.LoadScene(sceneNumberToLoad);
    //        return sceneNumberToLoad;
    //    }
    //    else if (SceneManager.GetActiveScene().buildIndex == sceneNumberToLoad)
    //    {
    //        //SceneManager.LoadScene(0);
    //        return 0;
    //    }
    //    return sceneNumberToLoad;
    //}
}
