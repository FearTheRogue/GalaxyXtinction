using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float minModifier;
    [SerializeField] private float maxModifier;
    [SerializeField] private float speed;

    [SerializeField] private float moveToPlayer;
    [SerializeField] private float currentMoveToPlayer;
    [SerializeField] private float moveToPlayerSpeed;

    Vector3 velocity = Vector3.zero;

    private void Update()
    {
        UpdateTarget();

        if (target != null)
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
        else
            transform.position += transform.forward * speed;
    }

    private void UpdateTarget()
    {
        currentMoveToPlayer = Mathf.MoveTowards(currentMoveToPlayer, moveToPlayer, moveToPlayerSpeed * Time.deltaTime);

        if(currentMoveToPlayer == moveToPlayer)
        {
            target = GameObject.Find("Drop Loot Tracker").gameObject.transform;
        }
    }
}
