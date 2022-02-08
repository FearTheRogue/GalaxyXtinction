using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] warpPortals;
    private WarpPortal portal;
    [SerializeField] private int currentWarpNumber;

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

        if (warpPortals.Length == 0)
        {
            warpPortals = GameObject.FindGameObjectsWithTag("Warp");
        }

        if (portal == null)
        {
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene has loaded");
        
        warpPortals = GameObject.FindGameObjectsWithTag("Warp");

        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i].GetComponent<WarpPortal>().warpPortalNumber == currentWarpNumber)
            {
                portal = warpPortals[i].GetComponent<WarpPortal>();

                player = GameObject.FindGameObjectWithTag("Player");

                player.transform.position = portal.GetSpawnPoint().position;
                player.transform.rotation = portal.GetSpawnPoint().rotation;
            }
        }
    }

    public void StartToLoadLevel(int levelToLoad)
    {
        SceneToLoad(levelToLoad);
        //SceneManager.LoadScene(levelToLoad);
        StartCoroutine(LoadSceneAsync(levelToLoad));
    }

    private IEnumerator LoadSceneAsync(int levelToLoad)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadAsync.isDone)
        {
            yield return null;
        }

        SceneManager.LoadScene(levelToLoad);
    }

    public int SceneToLoad(int sceneNumberToLoad)
    {
        currentWarpNumber = sceneNumberToLoad;
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //SceneManager.LoadScene(sceneNumberToLoad);
            return sceneNumberToLoad;
        }
        else if (SceneManager.GetActiveScene().buildIndex == sceneNumberToLoad)
        {
            //SceneManager.LoadScene(0);
            return 0;
        }
        return sceneNumberToLoad;
    }
}
