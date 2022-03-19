using UnityEngine;

public enum Behaviour { Idle, Wandering, Pursue, Attack };

public class EnemyMovement : MonoBehaviour
{
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
    private bool isAtDestination, hasWanderPos;

    [Header("Pursue Behaviour")]
    [SerializeField] private float distanceFromPlayer;

    [Header("Attack Behaviour")]
    [SerializeField] private float missileAttackRange = 100f;
    [SerializeField] private float homingMissileAttackRange = 150f;

    private Vector3 wanderLocation;
    private Vector3 target;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Weapon weapon;
    private HealthSystem health;

    [SerializeField] private RaycastHit hit;

    private void Awake()
    {
        isAtDestination = false;

        //weapon = GameObject.Find("Weapon").GetComponent<Weapon>();
        health = GetComponent<HealthSystem>();
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

    public void SetSpawnedEnemyLocation()
    {
        startingLocation = transform.position + new Vector3(Random.Range(50, 200), Random.Range(100,250), 0);
    }

    private void Update()
    {
        if(health.GetCurrentHealth() < 0)
        {
            health.Dead();
        }

        if (!canMove)
            return;

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

        if (interpolatingValue <= 1f)
        {
            interpolatingValue += 0.01f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            health.TakeDamage(100);
        }
    }

    private void MoveToLocation()
    {
        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, interpolatingValue * forwardSpeed, forwardAcceleration * Time.deltaTime);
        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
    }

    private void StartIdleBehaviour()
    {
        currentBehaviour = Behaviour.Idle;

        targetIdleTime = Random.Range(minIdleTime, maxIdleTime);
    }

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
        if (playerTransform == null)
            return;

        target = playerTransform.position;

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

    private bool IsPlayerInRange()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(gameObject.transform.position, transform.forward, out hit, layerMask))
        //{
        //    Debug.DrawLine(transform.position, hit.point, Color.blue);

        //    Physics.IgnoreCollision(gameObject.transform.Find("Ship Model").GetComponent<MeshCollider>(), hit.collider);

        //    if (Physics.Raycast(transform.position, hit.point, missileAttackRange))
        //    {
        //        Debug.DrawLine(transform.position, hit.point, Color.green);

        //        weapon.ShootMissile();
        //        Debug.Log("Shooting Missile");
        //    }
        //    else if (Physics.Raycast(transform.position, hit.point, homingMissileAttackRange))
        //    {
        //        Debug.DrawLine(transform.position, hit.point, Color.red);

        //        weapon.ShootHomingMissile();
        //        Debug.Log("Shooting Homing Missile");
        //    }
        //    Debug.Log("Result True - Tag Collided with: " + hit.transform.GetComponent<MeshCollider>());
        //    return true;
        //}

        //if (Physics.Raycast(transform.position, transform.forward, out hit, layerMask))
        //{
        //    Debug.DrawRay(gameObject.transform.position, hit.point, Color.red);

        //    if (!hit.collider.CompareTag("Player"))
        //    {
        //        Debug.Log("Tag Collided with: " + hit.collider.tag);
        //        return false;
        //    }

        //    return true;

        //}
        //Debug.Log("Result False - Tag Collided with: " + hit.transform.GetComponent<MeshCollider>());
        //return false;

        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, mask))
        {
            //Debug.Log("GameObject hit: " + hit.transform.name + " with a distance of: " + hit.distance);

            return true;
        }

        return false;
    }

    private void AttackPlayer()
    {
        //currentBehaviour = Behaviour.Attack;
        if (weapon.GetNumOfWeapons() > 1)
        {
            if (hit.distance <= homingMissileAttackRange && hit.distance >= missileAttackRange)
            {
                Debug.Log("Shooting Homing Missile");
                weapon.ShootHomingMissile();
            }
        }

        if (hit.distance <= missileAttackRange)
        {
            weapon.ShootMissile();
            Debug.Log("Shooting Normal Missile");
        }

        //Debug.Log("Distance: " + dist);
    }

    private float DistanceFromPlayer()
    {
        Vector3 targetPos = playerTransform.position;
        float dist = Vector3.Distance(transform.position, targetPos);

        return dist;
    }

    private float DistanceFromTarget()
    {
        Vector3 targetPos = target;
        float dist = Vector3.Distance(transform.position, targetPos);

        return dist;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        playerTransform = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        playerTransform = null;
    }
}