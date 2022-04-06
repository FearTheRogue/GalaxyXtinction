using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveManager : MonoBehaviour
{
    [SerializeField] private GameObject hivePrefab;
    [SerializeField] private GameObject[] hiveSpawnerPos;

    [SerializeField] private int[] enemy1AmountToSpawn;
    [SerializeField] private int[] enemy2AmountToSpawn;

    private Spawner spawner;

    private void Start()
    {
        //foreach (GameObject ship in hiveSpawnerPos)
        //{
        //    Quaternion rot = Random.rotation;
        //    GameObject hive = Instantiate(hivePrefab, ship.transform.position, new Quaternion(0, rot.y, 0, 0), ship.transform);
        //}

        for (int i = 0; i < hiveSpawnerPos.Length; i++)
        {
            Quaternion rot = Random.rotation;
            GameObject hive = Instantiate(hivePrefab, hiveSpawnerPos[i].transform.position, new Quaternion(0, rot.y, 0, 0), hiveSpawnerPos[i].transform);

            spawner = hive.GetComponentInChildren<Spawner>();
            spawner.destroyerAmountToSpawn = enemy1AmountToSpawn[i];
            spawner.assaultAmountToSpawn = enemy2AmountToSpawn[i];
        }
    }


}
