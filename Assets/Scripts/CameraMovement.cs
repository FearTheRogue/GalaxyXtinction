using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles the camera position for the players ship.
/// The AdjustCamera() method is called and updated using the FOV target set in the Inspector
/// 
/// </summary>

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private AnimationCurve camCurve;
    public float startFOV;
    public float normalSpeedFOV;
    public float boostSpeedFOV;
    [SerializeField] private float fovT;

    public void AdjustCamera(float target)
    {
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, target, camCurve.Evaluate(fovT * Time.deltaTime));
    }
}
