using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the target squares alpha
/// </summary>

public class TargetHandler : MonoBehaviour
{
    private Target target;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        target = other.GetComponent<Collider>().gameObject.transform.parent.GetComponent<Target>();

        Debug.Log("Target: " + target);

        target.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        target = other.GetComponent<Collider>().gameObject.transform.parent.GetComponent<Target>();

        Debug.Log("Target: " + target);

        target.enabled = true;
    }
}
