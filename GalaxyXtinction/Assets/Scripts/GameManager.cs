using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player;
    public GameObject[] warpPortals;

    public GameObject playerModel, fpsModel;

    public WarpPortal portal;

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

        if(portal == null)
        {
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            player.transform.rotation = Quaternion.Inverse(player.transform.localRotation); 
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
                portal = warpPortals[i].GetComponent<WarpPortal>();

                if (portal.isInSpace)
                {
                    
                    //player.transform.position = new Vector3(warpPortals[i].transform.position.x, warpPortals[i].transform.position.y, (warpPortals[i].transform.position.z - 24.0f));
                    //player.transform.rotation = Quaternion.LookRotation(player.transform.position - warpPortals[i].transform.position);
                    
                    Vector3 newPos = new Vector3((warpPortals[i].transform.position.x + 12.0f), warpPortals[i].transform.position.y, (warpPortals[i].transform.position.z - 24.0f));
                    Quaternion newRot = Quaternion.LookRotation(player.transform.position - warpPortals[i].transform.position);

                    Instantiate(playerModel, newPos, newRot);
                    
                    Debug.Log("Player returning to Space");
                }
                else if (!portal.isInSpace)
                {
                    //player.transform.position = warpPortals[i].transform.position;
                    Vector3 newPos = warpPortals[i].transform.position;

                    Instantiate(fpsModel, newPos, Quaternion.identity);

                    Debug.Log("Player entering planet");
                }
            }
        }
    }

    public void SceneToLoad(int sceneNumberToLoad)
    {
        currentWarpNumber = sceneNumberToLoad;
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(sceneNumberToLoad);
        }
        else if (SceneManager.GetActiveScene().buildIndex == sceneNumberToLoad)
        {
            SceneManager.LoadScene(0);
        }
    }
}
