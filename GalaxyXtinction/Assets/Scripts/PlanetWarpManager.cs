using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetWarpManager : MonoBehaviour
{
    [SerializeField] private WarpHandler warpToPlanetTrigger;
    [SerializeField] public string planetName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        warpToPlanetTrigger.OnPlayerEnter += TravelToPlanet;
    }

    private void OnDisable()
    {
        warpToPlanetTrigger.OnPlayerEnter -= TravelToPlanet;
    }

    private void TravelToPlanet()
    {
        SceneManager.LoadScene(planetName);
    }
}
