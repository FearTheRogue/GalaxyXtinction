using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpHandler : MonoBehaviour
{
    public int sceneToLoadIndex;
    public bool isInSpace, isInBox;
    [SerializeField] Transform spawnPoint;

    private void Start()
    {
        isInBox = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " has entered the box");

        if (other.tag != "Player")
            return;

        isInBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has exited the box");

        if (other.tag != "Player")
            return;

        isInBox = false;
    }

    private void BeginTravel()
    {
        GameManager.instance.StartToLoadLevel(sceneToLoadIndex);
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    private void Update()
    {
        if (isInBox && !isInSpace && Input.GetKeyDown(KeyCode.R))
        {
            BeginTravel();
        }
        else if(isInBox && isInSpace)
        {
            BeginTravel();
        }
    }
}
