using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
    }

    private void Start()
    {
        for (int i = 0; i < hiveSpawnerPos.Length; i++)
        {
            if (!hasBeenDestroyed[i])
            {
                Quaternion rot = Random.rotation;
                GameObject hive = Instantiate(hivePrefab, hiveSpawnerPos[i].transform.position, new Quaternion(0, rot.y, 0, 0), hiveSpawnerPos[i].transform);

                spawner = hive.GetComponentInChildren<Spawner>();
                spawner.destroyerAmountToSpawn = enemy1AmountToSpawn[i];
                spawner.assaultAmountToSpawn = enemy2AmountToSpawn[i];
            }
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= maxTime)
        {
            CanHiveShipSpawn();

            currentTime = 0;
        }

        if (hasBeenDestroyed[0] && hasBeenDestroyed[1] && hasBeenDestroyed[2])
        {
            InGameMenu.instance.PlayerHasWon = true;
        }
    }

    public void SetHiveShipSpawner()
    {
        CanHiveShipSpawn();

        for (int i = 0; i < hasBeenDestroyed.Length; i++)
        {
            PlayerPrefs.SetInt("Spawning-" + i, (hasBeenDestroyed[i] ? 1 : 0));
        }
    }

    public void GetHiveShipSpawner()
    {
        for (int i = 0; i < hasBeenDestroyed.Length; i++)
        {
            hasBeenDestroyed[i] = (PlayerPrefs.GetInt("Spawning-" + i) != 0);
        }
    }

    public void ResetHiveShipSpawner()
    {
        for (int i = 0; i < hasBeenDestroyed.Length; i++)
        {
            hasBeenDestroyed[i] = false;
        }
    }

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
