using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles enemy loot system, when eliminated.
/// 
/// </summary>

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
        // Drops loot after health is 0
        if(health.GetCurrentHealth() <= 0)
        {
            DropLoot();
        }
    }

    // Handles the dropping loot mechanic
    public void DropLoot()
    {
        // Randomises the amount of loot dropped 
        lootSpawned = Random.Range(minLoot, maxLoot);

        for (int i = 0; i < lootSpawned; i++)
        {
            GameObject loot = Instantiate(objectToDrop, transform.position, Quaternion.identity);

            // Sets a random rotation on X and Y
            float randomYDir = Random.Range(0f, 360f);
            float randomXDir = Random.Range(10f, -40f);

            loot.transform.rotation = Quaternion.Euler(randomXDir, randomYDir, 0);
        }
    }
}
