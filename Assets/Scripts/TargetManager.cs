using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the targetting the player gameobject.
/// Scripts 'EnemyMovement' use this script to set the target.
/// 
/// </summary>

public class TargetManager : MonoBehaviour
{
    private EnemyMovement movement;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float currentCountdown;
    [SerializeField] private float maxCountdown;
    [SerializeField] private float countdownSpeed;
    [SerializeField] private bool canStartCountdown;

    public bool enemySpawnedFromSpawner = false;

    private Spawner spawner;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();

        if (movement == null)
        {
            Debug.LogWarning("EnemyMovement script is not found");
        }

        canStartCountdown = false;

        spawner = transform.parent.GetComponent<Spawner>();
    }

    private void Start()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (canStartCountdown)
            StartCoroutine(StartCountdown());

        if(enemySpawnedFromSpawner)
            StartCoroutine(LocatePlayerFromSpawning(spawner.targetObject));
    }

    // Coroutine method, after a delay removes the target transform
    IEnumerator StartCountdown()
    {
        currentCountdown = Mathf.MoveTowards(currentCountdown, 0, countdownSpeed * Time.deltaTime);
        yield return null;

        if (currentCountdown <= 0)
            RemovePlayerTarget();
    }

    // Coroutine method, after enemy has been spawn, and another delay, the target transform is set
    public IEnumerator LocatePlayerFromSpawning(GameObject target)
    {
        GameObject player = target;
        yield return new WaitForSeconds(2);

        SetPlayerTarget(player.transform);

        enemySpawnedFromSpawner = false;
    }

    // Sets the target transform
    public void SetPlayerTarget(Transform transform)
    {
        canStartCountdown = false;
        currentCountdown = maxCountdown;
        playerTransform = transform;
    }

    // Removes target transform
    private void RemovePlayerTarget()
    {
        canStartCountdown = false;
        playerTransform = null;
    }

    // Returns a bool if target is assigned
    public bool IsTargetIdenified()
    {
        if (playerTransform != null)
            return true;

        return false; // returns false if player transform is null
    }

    // returns the player transform
    public Transform GetTargetPlayer()
    {
        return playerTransform;
    }

    // After trigging, sets the target transform
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        StopCoroutine(StartCountdown());
        SetPlayerTarget(other.transform);
    }

    // After exiting the trigger, a countdown begins
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        canStartCountdown = true;
    }
}
