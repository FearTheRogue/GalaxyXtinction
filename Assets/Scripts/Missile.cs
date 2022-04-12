using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float fireRate;
    [SerializeField] public float damage;

    [Header("Homing Missile")]
    [SerializeField] private bool isHoming;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ShipController player;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float timeToDestroy;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 15f;

    [Header("Prediction")]
    [SerializeField] private float maxDistancePredict = 100f;
    [SerializeField] private float minDistancePredict = 5f;
    [SerializeField] private float maxTimePrediction = 5f;
    private Vector3 standardPrediction, deviatedPrediction;

    [Header("Deviation")]
    [SerializeField] private float deviationAmount = 50f;
    [SerializeField] private float deviationSpeed = 2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<ShipController>();
    }

    private void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        timeToDestroy -= Time.deltaTime;

        if (timeToDestroy <= 0)
            MissileDie();
    }

    private void FixedUpdate()
    {
        if (!isHoming)
            return;

        if (!AudioManager.instance.IsClipPlaying("Homing Missile Engine"))
            AudioManager.instance.Play("Homing Missile Engine");

        rb.velocity = transform.forward * speed;

        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, player.transform.position));
        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
    }

    private void RotateRocket() 
    {
        var heading = deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTme = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

        standardPrediction = player.Rb.position + player.Rb.velocity * predictionTme;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

        deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void OnCollisionEnter(Collision other)
    {
        speed = 0;

        HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();

        if (healthSystem != null)
        {
            healthSystem.TakeDamage((int)damage);
        }

        MissileDie();
    }

    private void MissileDie()
    {
        if (AudioManager.instance.IsClipPlaying("Homing Missile Engine"))
            AudioManager.instance.Stop("Homing Missile Engine");

        if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(standardPrediction, deviatedPrediction);
    }
}
