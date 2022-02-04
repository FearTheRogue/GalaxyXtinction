using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpHandler : MonoBehaviour
{
    public int warpPortalNumber;
    public bool isInSpace, isInBox;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DisplayText text;

    UnityEvent travel = new UnityEvent();
    UnityEvent helpText = new UnityEvent();

    private void Start()
    {
        travel.AddListener(ReturnToSpace);
        //helpText.AddListener(text.IsTestVisible);

        isInBox = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player has entered the box");

        if (other.tag != "Player")
            return;

        travel.AddListener(ReturnToSpace);
        //helpText.Invoke();
        isInBox = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has exited the box");

        if (other.tag != "Player")
            return;

        travel.RemoveListener(ReturnToSpace);
        //helpText.Invoke
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
