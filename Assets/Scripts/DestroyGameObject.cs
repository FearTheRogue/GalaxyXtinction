using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;

    private void Update()
    {
        Destroy(gameObject, (timeToDestroy * Time.deltaTime));
    }
}
