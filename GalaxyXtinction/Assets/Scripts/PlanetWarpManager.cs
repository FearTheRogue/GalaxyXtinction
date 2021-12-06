using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetWarpManager : MonoBehaviour
{
    public static PlanetWarpManager instance;

    [SerializeField] private WarpHandler warpToPlanetTrigger; //warpToSpaceTrigger;
    [SerializeField] public string planetName;

    private Vector3 returningWarpPos;
    private bool isOnPlanet = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (!isOnPlanet)
        {
            warpToPlanetTrigger.OnPlayerEnter += TravelToPlanet;
            
            isOnPlanet = true;
        }
        else
        {
            // warpToSpaceTrigger.OnPlayerEnter += TravelToSpace;
            this.enabled = false;
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
        SceneManager.LoadScene(planetName);
    }

    private void TravelToSpace()
    {
        //returningWarpPos = new Vector3(warpToSpaceTrigger.warpPos.x, warpToSpaceTrigger.warpPos.y, warpToSpaceTrigger.warpPos.z);
    }
}
