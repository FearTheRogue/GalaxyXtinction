using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// A video tutorial was used as an initial starting point for this script
/// 
/// Tutoral Video: https://www.youtube.com/watch?v=7fdSYc8WElo
///
/// The project does not use this script.
/// 
/// </summary>

public class Orbit : MonoBehaviour
{
    public Transform orbitingObject;
    public Ellipse orbitPath;

    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;

    private void Start()
    {
        if(orbitingObject == null)
        {
            orbitActive = false;
            return;
        }

        SetOrbitingObjectPosition();

        StartCoroutine(AnimateOrbit());
    }

    void SetOrbitingObjectPosition()
    {
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }

    IEnumerator AnimateOrbit()
    {
        if (orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }

        float orbitSpeed = 1f / orbitPeriod;

        while (orbitActive)
        {
            orbitProgress += orbitSpeed * Time.deltaTime;
            orbitProgress %= 1f;
            SetOrbitingObjectPosition();

            yield return null;
        }
    }
}
