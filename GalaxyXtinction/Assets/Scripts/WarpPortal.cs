using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpPortal : MonoBehaviour
{
    public int warpPortalNumber;
    public bool isInSpace, isInBox;
    [SerializeField] Transform spawnPoint;

    UnityEvent travel = new UnityEvent();

    private void Start()
    {
        travel.AddListener(ReturnToSpace);

        isInBox = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player has entered the box");

        if (other.tag != "Player")
            return;

        travel.AddListener(ReturnToSpace);
        isInBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has exited the box");

        if (other.tag != "Player")
            return;

        travel.RemoveListener(ReturnToSpace);
        isInBox = false;
    }

    private void ReturnToSpace()
    {
        if (isInSpace)
        {
            GameManager.instance.SceneToLoad(warpPortalNumber);
        }
        else if (!isInSpace)
        {
            GameManager.instance.SceneToLoad(warpPortalNumber);
        }
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    private void Update()
    {
        if (isInBox && !isInSpace && Input.GetKeyDown(KeyCode.C) && travel != null)
        {
            travel.Invoke();
        }
        else if(isInBox && isInSpace && travel != null)
        {
            travel.Invoke();
        }
    }
}
