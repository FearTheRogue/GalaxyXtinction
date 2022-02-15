using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] warpPortals;
    private WarpHandler portal;
    [SerializeField] private int currentWarpNumber;

    Coroutine loadingSceneInProgress;

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

        if (portal == null)
        {
            return;
        }
    }


    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnLevelFinishedLoading;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    //}

    private void OnLevelFinishedLoading(int comingFromSceneIndex)
    {
        Debug.Log("Scene has loaded");

        warpPortals = GameObject.FindGameObjectsWithTag("Warp");

        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i].GetComponent<WarpHandler>().sceneToLoadIndex == comingFromSceneIndex)
            {
                portal = warpPortals[i].GetComponent<WarpHandler>();

                player = GameObject.FindGameObjectWithTag("Player");

                if (player.GetComponent<CharacterController>() != null)
                {
                    player.GetComponent<CharacterController>().enabled = false;
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
        if(loadingSceneInProgress != null)
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
        PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene(sceneToBeLoaded);
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
