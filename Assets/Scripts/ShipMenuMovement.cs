using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMenuMovement : MonoBehaviour
{
    [SerializeField] private GameObject ship;
    [SerializeField] private ParticleSystem fx;
    [SerializeField] private float speed;

    [SerializeField] private bool hasFinishedEntering;

    private Camera cam;

    private void Awake()
    {
        fx.Stop();
        cam = Camera.main;
    }

    private void Update()
    {
        if (!hasFinishedEntering)
            return;

        cam.transform.parent = this.transform;

        fx.Play();

        //fx.loop = true;

        transform.position -= transform.forward * speed * Time.deltaTime;
    }

    public void StartMoving()
    {
        hasFinishedEntering = true;
        Debug.Log("Starting");
    }
}
