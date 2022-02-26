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
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float minIdleTime, maxIdleTime, currentIdleTime, targetIdleTime;

    [Range(-1, 1)]
    [SerializeField] private float interpolatingValue;

    private Vector3 wanderLocation;
    private Vector3 target;
    private Transform playerTransform;

    [Header("Weapon Properties")]
    [SerializeField] private Transform[] missileSpawnPoint;
    [SerializeField] private float attackRange = 100f;
    [SerializeField] private GameObject missilePrefab;
    
    private float timeToFire = 0;
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

                UpdateIdleBehaviour();

                if(playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                } else
                {
                    if(currentIdleTime >= targetIdleTime)
                    {
                        currentBehaviour = Behaviour.Wandering;
                    }
                }
                break;

            case Behaviour.Wandering:

                if(playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                }
                else
                {
                    if (!hasWanderPos || !isAtDestination)
                    {
                        StartWandering();
                        currentIdleTime = 0f;
                    }
                    else
                    {
                        UpdateWanderingPos();
                    }
                }
                break;

            case Behaviour.Pursue:

                if(playerTransform == null)
                {
                    StartIdleBehaviour();
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

        if (interpolatingValue <= 1f)
        {
            interpolatingValue += 0.01f;
        }
    }

    private void MoveToLocation()
    {
        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, interpolatingValue * forwardSpeed, forwardAcceleration * Time.deltaTime);
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

    private void StartIdleBehaviour()
    {
        currentBehaviour = Behaviour.Idle;

        targetIdleTime = Random.Range(minIdleTime, maxIdleTime);
    }

    private void UpdateIdleBehaviour()
    {
        currentIdleTime += Time.deltaTime;
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
            //StartLooking();
            //transform.rotation = Quaternion.RotateTowards(target) * rotationSpeed;

            Debug.DrawRay(transform.position, Vector3.forward, Color.black);

            if (DistanceFromTarget() >= 1f && !isAtDestination)
            {
                MoveToLocation();
            } 
            else
            { 
                isAtDestination = true;
                StartIdleBehaviour();
            }
        }
    }

    void UpdateWanderingPos()
    {
        isAtDestination = false;
        hasWanderPos = false;

        StartIdleBehaviour();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawSphere(wanderLocation, 1f);
    }

    private void UpdatePursue()
    {
        target = playerTransform.position;

        //transform.LookAt(target);
        Quaternion rotation;
        Vector3 direction;
        direction = (playerTransform.position - transform.position).normalized;
        rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);

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
