using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player;
    public GameObject[] warpPortals;
    //public string sceneName;

    public int currentWarpNumber;

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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (warpPortals.Length == 0)
        {
            warpPortals = GameObject.FindGameObjectsWithTag("Warp");
        }
    }

    //private void OnLevelWasLoaded()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    warpPortals = GameObject.FindGameObjectsWithTag("Warp");

    //    for (int i = 0; i < warpPortals.Length; i++)
    //    {
    //        if (warpPortals[i].GetComponent<WarpPortal>().warpPortalNumber == currentWarpNumber)
    //        {
    //            player.transform.position = warpPortals[i].transform.position;
    //        }
    //    }
    //}

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

        player = GameObject.FindGameObjectWithTag("Player");
        warpPortals = GameObject.FindGameObjectsWithTag("Warp");

        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i].GetComponent<WarpPortal>().warpPortalNumber == currentWarpNumber)
            {
                player.transform.position = warpPortals[i].transform.position;

                Debug.Log("Player position: " + player.transform.position);
            }
        }
    }

    public void SceneToLoad(int sceneNumberToLoad)
    {
        currentWarpNumber = sceneNumberToLoad;
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(0);
        }
    }
}
