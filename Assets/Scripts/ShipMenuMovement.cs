using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the Main menu Ship Movement.
/// 
/// </summary>

public class ShipMenuMovement : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    [SerializeField] private ParticleSystem fx;
    [SerializeField] private float speed;

    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject[] itemsToSpawn;
    [SerializeField] private GameObject selectedItemToSpawn;

    private bool hasFinishedEntering;

    [SerializeField] private float currentSpawningTime = 0;
    [SerializeField] private float maxSpawningTime;

    private Camera cam;

    private void Awake()
    {
        fx.Stop();
        cam = Camera.main;
    }

    private void Update()
    {
        if (!AudioManager.instance.IsClipPlaying("Thruster Boost"))
        {
            AudioManager.instance.Play("Thruster Boost");
        }

        // Once the animation event is trigger to true
        if (!hasFinishedEntering)
            return;

        cam.transform.parent = this.transform;

        fx.Play();
        transform.position -= transform.forward * speed * Time.deltaTime;

        // Increasing currentSpawningTime
        currentSpawningTime += Time.deltaTime;

        // If currentSpawningTime reaches the maxSpawningTime
        if(currentSpawningTime > maxSpawningTime)
        {
            // Selects a new Item to spawn
            SelectRandomItem();
            
            // Instantiates the item
            GameObject item = Instantiate(selectedItemToSpawn, new Vector3(spawner.transform.position.x, Random.Range(spawner.transform.position.y - 15, spawner.transform.position.y + 30),
                spawner.transform.position.z), Random.rotation);
            item.transform.parent = null;

            // Resets the currentSpawningTime
            currentSpawningTime = 0;
        }
    }

    // Selects a random object to spawn
    private void SelectRandomItem()
    {
        int item = Random.Range(0, itemsToSpawn.Length);

        selectedItemToSpawn = itemsToSpawn[item];
    }

    // Method is used by an animation event
    public void StartMoving()
    {
        if (hasFinishedEntering)
            return;

        hasFinishedEntering = true;
    }
}
