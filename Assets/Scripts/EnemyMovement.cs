using UnityEngine;

/// <summary>
/// 
/// Handles the enemy movement using a switch case and enums to represent the different
/// behaviour states.
/// 
/// </summary>

// Different behaviours
public enum Behaviour { Idle, Wandering, Pursue, Attack };

public class EnemyMovement : MonoBehaviour
{
    // Sets the initial behaviour to Idle 
    [Header("Current Behaviour")]
    [SerializeField] private Behaviour currentBehaviour = Behaviour.Idle;

    [SerializeField] private bool canMove = true;

    private Vector3 startingLocation;
    public bool hasBeenSpawned;

    [Header("Ship Properties")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private SphereCollider detector;
    [SerializeField] private LayerMask mask;

    [Range(0f,1f)]
    [SerializeField] private float rotationSpeed;
    
    [Range(-1, 1)]
    [SerializeField] private float interpolatingValue;

    [Header("Idle Behaviour")]
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float currentIdleTime;
    [SerializeField] private float targetIdleTime;

    [Header("Wander Behaviour")]
    [SerializeField] private float wanderRange = 5f;
    [SerializeField] private float distanceFromTarget;
    public bool isAtDestination, hasWanderPos;

    [Header("Pursue Behaviour")]
    [SerializeField] private float distanceFromPlayer;

    [Header("Attack Behaviour")]
    [SerializeField] private float missileAttackRange = 100f;
    [SerializeField] private float homingMissileAttackRange = 150f;

    private Vector3 wanderLocation;
    private Vector3 target;

    [SerializeField] private Weapon weapon;
    private HealthSystem health;

    [SerializeField] private RaycastHit hit;

    private DropLookSystem dropLoot;
    private TargetManager targetManager;

    private void Awake()
    {
        isAtDestination = false;

        health = GetComponent<HealthSystem>();
        targetManager = GetComponent<TargetManager>();
        dropLoot = GetComponent<DropLookSystem>();
    }

    private void Start()
    {
        if (hasBeenSpawned)
        {
            SetSpawnedEnemyLocation();
            return;
        }

        startingLocation = transform.position;
    }

    // Sets a location above the enemies position after being spawned
    public void SetSpawnedEnemyLocation()
    {
        startingLocation = transform.position + new Vector3(Random.Range(50, 200), Random.Range(100,250), 0);
    }

    private void Update()
    {
        // For testing purposes
        if (!canMove)
            return;

        // Handles the behaviour
        switch (currentBehaviour)
        {
            case Behaviour.Idle:

                UpdateIdleBehaviour();

                if(targetManager.IsTargetIdenified()) //playerTransform != null
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

                if(targetManager.IsTargetIdenified()) //playerTransform != null
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

                if(!targetManager.IsTargetIdenified()) //playerTransform == null
                {
                    StartIdleBehaviour();
                }
                else
                {
                    if (IsPlayerInRange())
                    {
                        AttackPlayer();
                    }

                    UpdatePursue();
                }
                break;

            case Behaviour.Attack:

                UpdatePursue();

                //if(!IsPlayerInRange())
                //{
                //    currentBehaviour = Behaviour.Pursue;
                //} 

                break;
        }

        // Speed of the enemy
        if (interpolatingValue <= 1f)
        {
            interpolatingValue += 0.01f;
        }

        // Testing Purposes
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            health.TakeDamage(100);
        }
        */
    }
    
    private void MoveToLocation()
    {
        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, interpolatingValue * forwardSpeed, forwardAcceleration * Time.deltaTime);
        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
    }

    private void StartIdleBehaviour()
    {
        // Sets current behaviour
        currentBehaviour = Behaviour.Idle;

        // Random Idle time
        targetIdleTime = Random.Range(minIdleTime, maxIdleTime);
    }

    // Handles levelling out the enemy when in Idle
    private void UpdateIdleBehaviour()
    { 
        Quaternion levellingOut = new Quaternion(0, transform.rotation.y, transform.rotation.z,1.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, levellingOut, (rotationSpeed / 2.0f));

        currentIdleTime += Time.deltaTime;
    }

    private void StartWandering()
    {
        if (!isAtDestination && !hasWanderPos)
        {
            // Sets wander location
            wanderLocation = startingLocation + Random.insideUnitSphere * wanderRange;
            wanderLocation.y = Random.Range(startingLocation.y - 50f, startingLocation.y + 50f);

            target = wanderLocation;
            hasWanderPos = true;
        } 

        if (hasWanderPos)
        {
            UpdateRotation(target);

            if (DistanceFromTarget() >= distanceFromTarget && !isAtDestination)
            {
                MoveToLocation();
            } 
            else
            { 
                isAtDestination = true;
                currentForwardSpeed = 0f;
                StartIdleBehaviour();
            }
        }
    }

    // Rotates the enemy, specified by a speed value
    private void UpdateRotation(Vector3 posToRotate)
    {
        Quaternion rotation;
        Vector3 direction;
        direction = (posToRotate - transform.position).normalized;
        rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
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
        if (!targetManager.IsTargetIdenified()) //playerTransform == null
            return;

        target = targetManager.GetTargetPlayer().position;

        UpdateRotation(target);

        if (DistanceFromTarget() >= distanceFromPlayer)
        {
            isAtDestination = false;
            MoveToLocation();
        }
        else
        {
            isAtDestination = true;

            //currentBehaviour = Behaviour.Attack;
        }
    }

    // Checks if player is in range
    private bool IsPlayerInRange()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, mask))
        {
            return true;
        }

        return false;
    }

    // Handles Shooting when in range
    private void AttackPlayer()
    {
        //currentBehaviour = Behaviour.Attack;

        // Checks number of weapons
        if (weapon.GetNumOfWeapons() > 1)
        {
            if (hit.distance <= homingMissileAttackRange && hit.distance >= missileAttackRange)
            {
                weapon.ShootHomingMissile();
            }
        }

        if (hit.distance <= missileAttackRange)
        {
            weapon.ShootMissile();
        }
    }

    private float DistanceFromPlayer()
    {
        Vector3 targetPos = targetManager.GetTargetPlayer().position;
        float dist = Vector3.Distance(transform.position, targetPos);

        return dist;
    }

    // Distance from the enemy to target transforms
    private float DistanceFromTarget()
    {
        Vector3 targetPos = target;
        float dist = Vector3.Distance(transform.position, targetPos);

        return dist;
    }
}