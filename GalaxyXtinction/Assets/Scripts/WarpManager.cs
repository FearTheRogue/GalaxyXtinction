using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpManager : MonoBehaviour
{
    public static WarpManager instance;

    [SerializeField] private WarpHandler warpToPlanetTrigger;//, warpToSpaceTrigger;
    [SerializeField] public string planetName;

    private GameObject findWarpObjects;

    private Vector3 returningWarpPos;
    public bool isOnPlanet = false;

    void Awake()
    {
        findWarpObjects = GameObject.FindGameObjectWithTag("Warp");
        warpToPlanetTrigger = findWarpObjects.GetComponent<WarpHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnPlanet)
        {
            this.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (!isOnPlanet)
        {
            warpToPlanetTrigger.OnPlayerEnter += TravelToPlanet;
        }
        else
        {
            // warpToSpaceTrigger.OnPlayerEnter += TravelToSpace;
            isOnPlanet = false;
        }
    }

    private void OnDisable()
    {
        warpToPlanetTrigger.OnPlayerEnter -= TravelToPlanet;
        //warpToSpaceTrigger.OnPlayerEnter -= TravelToSpace;
    }

    private void TravelToPlanet()
    {
        isOnPlanet = true;
        SceneManager.LoadScene(planetName);
    }

    private void TravelToSpace()
    {
        //returningWarpPos = new Vector3(warpToSpaceTrigger.warpPos.x, warpToSpaceTrigger.warpPos.y, warpToSpaceTrigger.warpPos.z);
    }
}
