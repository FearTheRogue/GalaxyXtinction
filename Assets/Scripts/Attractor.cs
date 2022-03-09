using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float minModifier;
    [SerializeField] private float maxModifier;

    Vector3 velocity = Vector3.zero;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
    }
}
