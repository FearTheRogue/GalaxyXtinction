using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToSpawn;

    [SerializeField] private bool canSpawn;

    [SerializeField] public int destroyerAmountToSpawn;
    [SerializeField] public int assaultAmountToSpawn;

    private EnemyMovement enemyMovement;

    public GameObject targetObject;

    private TargetManager targetManager;

    private void Awake()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < destroyerAmountToSpawn; i++)
        {
            GameObject enemy;
            enemy = Instantiate(enemiesToSpawn[0], transform.position, Quaternion.identity, this.transform);

            enemyMovement = enemy.GetComponent<EnemyMovement>();
            enemyMovement.hasBeenSpawned = true;

            targetManager = enemy.GetComponent<TargetManager>();
            targetManager.enemySpawnedFromSpawner = true;

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

        //foreach (GameObject enemy in enemiesToSpawn)
        //{
        //    enemyMovement = enemy.GetComponent<EnemyMovement>();
        //    Instantiate(enemy, transform.position, Quaternion.identity, this.transform);
        //    enemyMovement.hasBeenSpawned = true;

        //    targetManager = enemy.GetComponent<TargetManager>();
        //    targetManager.enemySpawnedFromSpawner = true;

        //    yield return new WaitForSeconds(1.5f);
        //}

        canSpawn = false;
        yield return null;
    }

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