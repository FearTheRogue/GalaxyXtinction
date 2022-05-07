using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=OUB1l9i2Gxg&t=52s
/// 
/// It has been modified for the project so the gameobject does not travel to the player instantly.
/// Instead after a short time, the target is then assigned. Other modifications include that after 
/// spawning the game object it moves in the direction that its facing for a short while.
/// 
/// Rotation of the gameobject set in the 'DropLootSystem' script.
/// 
/// </summary>

public class Attractor : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float minModifier;
    [SerializeField] private float maxModifier;
    [SerializeField] private float speed;

    [SerializeField] private float moveToPlayer;
    [SerializeField] private float currentMoveToPlayer;
    [SerializeField] private float moveToPlayerSpeed;

    [SerializeField] private float healthModifier;

    Vector3 velocity = Vector3.zero;

    private void Update()
    {
        UpdateTarget();

        // When target has been set
        if (target != null)
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
        else
            // Travels in the forward direction
            transform.position += transform.forward * speed;
    }

    // Handles updating the target for the gameobject
    private void UpdateTarget()
    {
        // currentMoveToPlayer is used as a delay
        currentMoveToPlayer = Mathf.MoveTowards(currentMoveToPlayer, moveToPlayer, moveToPlayerSpeed * Time.deltaTime);

        // After the delay condition is met, set the gameobject's target
        if(currentMoveToPlayer == moveToPlayer)
        {
            target = GameObject.Find("Drop Loot Tracker").gameObject.transform;
        }
    }

    // Handles when the gameobject's collider triggers with a gameobject with the tag "Player"
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Creates a HealthSystem instance and stores the health component from the player
        HealthSystem health = other.transform.GetComponentInParent<HealthSystem>();

        // Applies health to the component by the healthModifier
        health.ApplyHealth((int)healthModifier);
    }
}
