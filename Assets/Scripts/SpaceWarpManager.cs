using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceWarpManager : MonoBehaviour
{
    [SerializeField] private WarpHandler warpToSpaceTrigger;
    //[SerializeField] private string backToSpace;

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
        warpToSpaceTrigger.OnPlayerEnter += TravelToSpace;
    }

    private void OnDisable()
    {
        warpToSpaceTrigger.OnPlayerEnter -= TravelToSpace;
    }

    private void TravelToSpace()
    {
        //SceneManager.LoadScene(backToSpace);
    }
}
