using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the Spawning of the enemies from the Hive Ship.
/// 
/// </summary>

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToSpawn;

    [SerializeField] private bool canSpawn;

    [SerializeField] public int destroyerAmountToSpawn;
    [SerializeField] public int assaultAmountToSpawn;

    private EnemyMovement enemyMovement;

    public GameObject targetObject;

    private TargetManager targetManager;

    private HealthSystem health;

    private void Awake()
    {
        StopAllCoroutines();

        health = gameObject.GetComponentInParent<HealthSystem>();
        health.canDie = false;
    }

    private void Update()
    {
        if(this.transform.childCount <= 0 && !canSpawn)
        {
            // has no enemies
            health.canDie = true;
        }
    }

    // Spawns each enemy in the array
    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < destroyerAmountToSpawn; i++)
        {
            GameObject enemy;
            enemy = Instantiate(enemiesToSpawn[0], transform.position, Quaternion.identity, this.transform);

            // Set the hasBeenSpawned to true
            enemyMovement = enemy.GetComponent<EnemyMovement>();
            enemyMovement.hasBeenSpawned = true;

            // Set the enemySpawnFromSpawn to true
            targetManager = enemy.GetComponent<TargetManager>();
            targetManager.enemySpawnedFromSpawner = true;

            // Wait
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < assaultAmountToSpawn; i++)
        {
            GameObject enemy;
            enemy = Instantiate(enemiesToSpawn[1], transform.position, Quaternion.identity, this.transform);

            enemyMovement = enemy.GetComponent<EnemyMovement>();
            enemyMovement.hasBeenSpawned = true;

            targetManager = enemy.GetComponent<TargetManager>();
            targetManager.enemySpawnedFromSpawner = true;

            yield return new WaitForSeconds(1f);
        }

        canSpawn = false;
        yield return null;
    }

    // After the player enters the trigger, starts to spawn enemies
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        targetObject = other.gameObject;

        if (canSpawn)
            StartCoroutine(SpawnEnemy());
    }
}