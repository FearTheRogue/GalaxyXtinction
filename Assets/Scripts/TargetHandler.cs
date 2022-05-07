using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the enabling or disabling of the 'target' script.
/// 
/// </summary>

public class TargetHandler : MonoBehaviour
{
    private Target target;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        // Assigns the 'Target' script from enemy gameobject
        target = other.GetComponent<Collider>().gameObject.transform.parent.GetComponent<Target>();

        target.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        // Assigns the 'Target' script from enemy gameobject
        target = other.GetComponent<Collider>().gameObject.transform.parent.GetComponent<Target>();

        target.enabled = true;
    }
}
