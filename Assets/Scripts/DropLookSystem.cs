using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLookSystem : MonoBehaviour
{
    private HealthSystem health;
    private EnemyMovement enemy;

    [SerializeField] private int minLoot;
    [SerializeField] private int maxLoot;

    [SerializeField] private int lootSpawned;
    [SerializeField] private GameObject objectToDrop;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        enemy = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if(health.GetCurrentHealth() <= 0)
        {
            DropLoot();
        }
    }

    //private void SpawnLoot()
    //{ 
    //    int amountToSpawn = Random.Range(minRate, maxRate);

    //    for (int i = 0; i <= amountToSpawn; i++)
    //    {
    //        GameObject obj = Instantiate(objectToDrop, transform.position + new Vector3(0, Random.Range(0, 2)), Quaternion.identity);

    //        obj.GetComponent<Attractor>().target = GameObject.FindGameObjectWithTag("Player").transform;
    //    }

    //    Debug.Log("Loot Spawned: " + amountToSpawn);
    //}

    public void DropLoot()
    {
        lootSpawned = Random.Range(minLoot, maxLoot);

        for (int i = 0; i < lootSpawned; i++)
        {
            GameObject loot = Instantiate(objectToDrop, transform.position, Quaternion.identity);

            float randomYDir = Random.Range(0f, 360f);
            float randomXDir = Random.Range(10f, -40f);

            loot.transform.rotation = Quaternion.Euler(randomXDir, randomYDir, 0);
        }
    }
}
