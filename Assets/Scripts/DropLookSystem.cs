using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLookSystem : MonoBehaviour
{
    private HealthSystem health;
    private EnemyMovement enemy;

    [SerializeField] private GameObject objectToDrop;

    [SerializeField] private int minRate;
    [SerializeField] private int maxRate;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        enemy = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if(health.GetCurrentHealth() <= 0)
        {
            SpawnLoot();
        }
    }

    private void SpawnLoot()
    { 
        int amountToSpawn = Random.Range(minRate, maxRate);

        for (int i = 0; i <= amountToSpawn; i++)
        {
            GameObject obj = Instantiate(objectToDrop, transform.position + new Vector3(0, Random.Range(0, 2)), Quaternion.identity);

            obj.GetComponent<Attractor>().target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Debug.Log("Loot Spawned: " + amountToSpawn);
    }
}
