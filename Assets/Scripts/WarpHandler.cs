using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// Handles the Warp Points Triggers.
/// 
/// </summary>

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
        //Debug.Log(other.gameObject.name + " has entered the box");

        // If the collider tag isnt Player, return
        if (other.tag != "Player")
            return;

        // isInBox is set to true when the player enters the collider
        isInBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has exited the box");

        // If the collider tag isnt Player, return
        if (other.tag != "Player")
            return;

        // isInBox is set to false when the player exits the collider
        isInBox = false;
    }

    // Starts the travelling process
    private void BeginTravel()
    {
        GameManager.instance.StartToLoadLevel(sceneToLoadIndex);
    }

    // Returns the Transform of the spawner gameobject
    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    private void Update()
    {
        // If the player is on planet
        if (isInBox && !isInSpace && Input.GetKeyDown(KeyCode.R))
        {
            BeginTravel();
        }
        // If the player is in space
        else if(isInBox && isInSpace)
        {
            BeginTravel();
        }
    }
}
