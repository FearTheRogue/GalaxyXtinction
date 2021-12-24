using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousArea : MonoBehaviour
{
    public static PreviousArea instance;

    public string areaTransitionName;

    public bool IsOnPlanet = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
