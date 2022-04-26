using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (!hasFinishedEntering)
            return;

        cam.transform.parent = this.transform;

        fx.Play();
        transform.position -= transform.forward * speed * Time.deltaTime;

        currentSpawningTime += Time.deltaTime;

        if(currentSpawningTime > maxSpawningTime)
        {
            SelectRandomItem();

            GameObject item = Instantiate(selectedItemToSpawn, new Vector3(spawner.transform.position.x, Random.Range(spawner.transform.position.y - 15, spawner.transform.position.y + 30),
                spawner.transform.position.z), Random.rotation);
            item.transform.parent = null;

            currentSpawningTime = 0;
        }
    }

    private void SelectRandomItem()
    {
        int item = Random.Range(0, itemsToSpawn.Length);

        selectedItemToSpawn = itemsToSpawn[item];
    }

    public void StartMoving()
    {
        if (hasFinishedEntering)
            return;

        hasFinishedEntering = true;
    }
}
