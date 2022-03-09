using UnityEngine;

public enum Behaviour { Idle, Wandering, Pursue, Attack };

public class EnemyMovement : MonoBehaviour
{
    [Header("Current Behaviour")]
    [SerializeField] private Behaviour currentBehaviour = Behaviour.Idle;

    private Vector3 startingLocation;

    [Header("Ship Properties")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float currentForwardSpeed;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private SphereCollider detector;

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
    public Transform playerTransform;

    private Weapon weapon;
    private HealthSystem health;

    private void Awake()
    {
        startingLocation = transform.position;

        isAtDestination = false;

        weapon = GetComponent<Weapon>();
        health = GetComponent<HealthSystem>();
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

        if(health.currentHealth <= 0)
        {
            health.Dead();
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
        }
    }

    private bool IsPlayerInRange()
    {
        RaycastHit hit;

        if (Physics.Raycast(gameObject.transform.position, transform.forward, out hit, detector.radius))
        {
            Debug.DrawLine(transform.position, hit.point, Color.blue);

            if(hit.distance <= homingMissileAttackRange && hit.distance >= missileAttackRange)
            {
                Debug.DrawLine(transform.position, hit.point, Color.white);

                weapon.ShootHomingMissile();
            }
            else if (hit.distance <= missileAttackRange)
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);

                weapon.ShootMissile();
            }

            //weapon.BurstShoot();

            return true;
        }
        //else if (Physics.Raycast(gameObject.transform.position, transform.forward, out hit, homingMissileAttackRange))
        //{
        //    Debug.DrawLine(gameObject.transform.position, hit.point, Color.gray);

        //    weapon.ShootHomingMissile();
        //    weapon.canHomingMissile = false;

        //    return true;
        //}

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
