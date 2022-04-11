using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //this.transform.parent = null;
    }

    private void Update()
    {
        if (canStartCountdown)
            StartCoroutine(StartCountdown());

        if(enemySpawnedFromSpawner)
            StartCoroutine(LocatePlayerFromSpawning(spawner.targetObject));
    }

    IEnumerator StartCountdown()
    {
        currentCountdown = Mathf.MoveTowards(currentCountdown, 0, countdownSpeed * Time.deltaTime);
        yield return null;

        if (currentCountdown <= 0)
            RemovePlayerTarget();
    }

    public IEnumerator LocatePlayerFromSpawning(GameObject target)
    {
        GameObject player = target;
        yield return new WaitForSeconds(2);

        SetPlayerTarget(player.transform);

        enemySpawnedFromSpawner = false;
    }

    public void SetPlayerTarget(Transform transform)
    {
        canStartCountdown = false;
        currentCountdown = maxCountdown;
        playerTransform = transform;
    }

    private void RemovePlayerTarget()
    {
        canStartCountdown = false;
        playerTransform = null;
    }

    public bool IsTargetIdenified()
    {
        if (playerTransform != null)
            return true;

        return false; // returns false if player transform is null
    }

    public Transform GetTargetPlayer()
    {
        return playerTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        StopCoroutine(StartCountdown());
        SetPlayerTarget(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        canStartCountdown = true;
    }
}
