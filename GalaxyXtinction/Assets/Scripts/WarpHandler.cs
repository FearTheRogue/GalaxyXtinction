using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpHandler : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        OnPlayerEnter.Invoke();
    }
}
