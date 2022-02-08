using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpHandler : MonoBehaviour
{
    private string warpTo;
    public UnityAction OnPlayerEnter;
    public Vector3 warpPos;

    // Start is called before the first frame update
    void Start()
    {
        warpPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

<<<<<<< Updated upstream
    private void OnTriggerEnter(Collider other)
=======
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
            //GameManager.instance.SceneToLoad(warpPortalNumber);
            GameManager.instance.StartToLoadLevel(warpPortalNumber);
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
>>>>>>> Stashed changes
    {
        if(other.tag == "Player")
        OnPlayerEnter.Invoke();
    }
}
