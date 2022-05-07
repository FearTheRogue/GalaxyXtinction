using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles Hive ship spawning into space scene.
/// Saves if the Hive ship has been destroyed.
/// 
/// </summary>

public class HiveManager : MonoBehaviour
{
    [SerializeField] private GameObject hivePrefab;
    [SerializeField] private GameObject[] hiveSpawnerPos;

    [SerializeField] private int[] enemy1AmountToSpawn;
    [SerializeField] private int[] enemy2AmountToSpawn;

    [SerializeField] private bool[] hasBeenDestroyed;

    [SerializeField] private float currentTime;
    [SerializeField] private float maxTime;

    private Spawner spawner;

    private void Start()
    {
        // Initially spawns the Hive ship if is hasn't been already destroyed
        for (int i = 0; i < hiveSpawnerPos.Length; i++)
        {
            if (!hasBeenDestroyed[i])
            {
                // Applies a random Y rotation
                Quaternion rot = Random.rotation;
                GameObject hive = Instantiate(hivePrefab, hiveSpawnerPos[i].transform.position, new Quaternion(0, rot.y, 0, 0), hiveSpawnerPos[i].transform);

                // Assigns the amount of enemies to spawn
                spawner = hive.GetComponentInChildren<Spawner>();
                spawner.destroyerAmountToSpawn = enemy1AmountToSpawn[i];
                spawner.assaultAmountToSpawn = enemy2AmountToSpawn[i];
            }
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // Checks after a set amount, if a Hive ship has been destroyed
        if (currentTime >= maxTime)
        {
            CanHiveShipSpawn();

            currentTime = 0;
        }

        // If each Hive Ship has been destroyed, player has won
        if (hasBeenDestroyed[0] && hasBeenDestroyed[1] && hasBeenDestroyed[2])
        {
            InGameMenu.instance.PlayerHasWon = true;
        }
    }

    // Saves to PlayerPrefs if the Hive ship has been destroyed 
    public void SetHiveShipSpawner()
    {
        CanHiveShipSpawn();

        for (int i = 0; i < hasBeenDestroyed.Length; i++)
        {
            PlayerPrefs.SetInt("Spawning-" + i, (hasBeenDestroyed[i] ? 1 : 0));
        }
    }

    // Gets the value from the PlayerPrefs if the Hive Ship has been destroyed
    public void GetHiveShipSpawner()
    {
        for (int i = 0; i < hasBeenDestroyed.Length; i++)
        {
            hasBeenDestroyed[i] = (PlayerPrefs.GetInt("Spawning-" + i) != 0);
        }
    }

    // Reset the Hive ships
    public void ResetHiveShipSpawner()
    {
        for (int i = 0; i < hasBeenDestroyed.Length; i++)
        {
            hasBeenDestroyed[i] = false;
        }
    }

    // Method that checks if a Hive ship has been destroyed
    public void CanHiveShipSpawn()
    {
        for (int i = 0; i < hiveSpawnerPos.Length; i++)
        {
            if (hiveSpawnerPos[i].gameObject.transform.childCount <= 0)
            {
                hasBeenDestroyed[i] = true;
            }
        }
    }
}
