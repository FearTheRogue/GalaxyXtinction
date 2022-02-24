using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum Behaviour { Idle, Wandering, Pursue, Attack };
    [SerializeField] private Behaviour currentBehaviour = Behaviour.Idle;

    private Vector3 startingLocation;

    [Header("Ship Properties")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float forwardAcceleration;

    [SerializeField] private float wanderRange = 5f;
    private Vector3 wanderLocation;
    private Vector3 target;
    private Transform playerTransform;

    [Header("Weapon Properties")]
    [SerializeField] private Transform[] missileSpawnPoint;
    [SerializeField] private float attackRange = 100f;
    [SerializeField] private GameObject missilePrefab;
    private float timeToFire = 0;

    [Range(-1, 1)]
    public float interpalatingValue;

    private bool isAtDestination, hasWanderPos;

    private void Awake()
    {
        startingLocation = transform.position;

        isAtDestination = false;
    }

    private void Update()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Idle:

                if(playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                } else
                {
                    currentBehaviour = Behaviour.Wandering;
                }
                break;

            case Behaviour.Wandering:

                if(playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                } else
                {
                    if (!hasWanderPos || !isAtDestination)
                        StartWandering();
                    else
                        UpdateWanderingPos();
                }
                break;

            case Behaviour.Pursue:

                if(playerTransform == null)
                {
                    currentBehaviour = Behaviour.Idle;
                }
                else
                {
                    if (IsPlayerInRange())
                    {
                        currentBehaviour = Behaviour.Attack;
                    }
                    UpdatePursue();
                }
                break;

            case Behaviour.Attack:

                UpdatePursue();

                if(!IsPlayerInRange())
                {
                    currentBehaviour = Behaviour.Pursue;
                }

                break;
        }

        //if (target == null)
        //{
        //    if (interpalatingValue >= 0)
        //    {
        //        interpalatingValue -= 0.1f;
        //    }
        //}

        if (interpalatingValue <= 1f)
        {
            interpalatingValue += 0.01f;
        }
    }

    private void MoveToLocation()
    {
        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, interpalatingValue * forwardSpeed, forwardAcceleration * Time.deltaTime);
        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
    }

    private void ShootPlayer()
    {
        if(Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / missilePrefab.GetComponent<Missile>().fireRate;
            
            foreach (Transform origin in missileSpawnPoint)
            {
                Instantiate(missilePrefab, origin.transform.position, origin.transform.rotation);
            }
            
        }
    }

    private void StartWandering()
    {
        if (!isAtDestination && !hasWanderPos)
        {
            wanderLocation = startingLocation + Random.insideUnitSphere * wanderRange;
            wanderLocation.y = Random.Range(startingLocation.y - 50f, startingLocation.y + 50f);

            target = wanderLocation;
            hasWanderPos = true;
        } 

        if (hasWanderPos)
        {
            transform.LookAt(target);

            Debug.DrawRay(transform.position, Vector3.forward, Color.black);

            if (DistanceFromTarget() >= 1f && !isAtDestination)
            {
                MoveToLocation();
            } 
            else
            { 
                isAtDestination = true;
            }
        }
    }

    void UpdateWanderingPos()
    {
        isAtDestination = false;
        hasWanderPos = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawSphere(wanderLocation, 1f);
    }

    private void UpdatePursue()
    {
        target = playerTransform.position;

        transform.LookAt(target);

        if (DistanceFromTarget() >= 30.0f)
        {
            isAtDestination = false;
            MoveToLocation();
        }
        else
        {
            isAtDestination = true;
        }
    }

    private bool IsPlayerInRange()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(gameObject.transform.position, transform.forward, out hit, attackRange))
        {
            Debug.DrawRay(gameObject.transform.position, hit.point);

            ShootPlayer();

            return true;
        }

        return false;
    }

    private float DistanceFromTarget()
    {
        Vector3 targetPos = target;
        float dist = Vector3.Distance(transform.position, targetPos);

        return dist;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            return;
        }

        playerTransform = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player")
        {
            return;
        }

        playerTransform = null;
    }
}
