using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
