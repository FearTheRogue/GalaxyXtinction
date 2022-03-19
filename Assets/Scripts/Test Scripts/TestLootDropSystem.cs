using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLootDropSystem : MonoBehaviour
{
    [SerializeField] private int minLoot;
    [SerializeField] private int maxLoot;

    [SerializeField] private int lootSpawned;
    [SerializeField] private GameObject objectToDrop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DropLoot();
            //StartCoroutine(DroppingLoot());
        }


    }

    private void DropLoot()
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

    IEnumerator DroppingLoot()
    {
        lootSpawned = Random.Range(minLoot, maxLoot);

        for (int i = 0; i < lootSpawned; i++)
        {
            GameObject loot = Instantiate(objectToDrop, transform.position, Quaternion.identity);

            float randomYDir = Random.Range(0f, 360f);
            float randomXDir = Random.Range(10f, -40f);

            loot.transform.rotation = Quaternion.Euler(randomXDir, randomYDir, 0);
        }

        yield return null;
    }
}
