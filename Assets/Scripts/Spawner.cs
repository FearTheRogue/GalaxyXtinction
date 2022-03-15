using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToSpawn;

    [SerializeField] private int destroyerAmountToSpawn;
    [SerializeField] private int assaultAmountToSpawn;

    private EnemyMovement enemyMovement;

    private void Awake()
    {

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
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        StartCoroutine(SpawnEnemy());
    }

}
