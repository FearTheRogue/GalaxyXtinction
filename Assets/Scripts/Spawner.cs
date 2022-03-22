using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToSpawn;

    [SerializeField] private bool canSpawn;

    [SerializeField] private int destroyerAmountToSpawn;
    [SerializeField] private int assaultAmountToSpawn;

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
        foreach (GameObject enemy in enemiesToSpawn)
        {
            enemyMovement = enemy.GetComponent<EnemyMovement>();
            Instantiate(enemy, transform.position, Quaternion.identity);
            enemyMovement.hasBeenSpawned = true;

            targetManager = enemy.GetComponent<TargetManager>();

            StartCoroutine(targetManager.LocatePlayerFromSpawning(targetObject));

            yield return new WaitForSeconds(2);
        }

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