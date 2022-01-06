using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    public int warpPortalNumber;
    public bool isInSpace;

    private void OnTriggerStay()
    {
        if (isInSpace)
        {
            GameManager.instance.SceneToLoad(warpPortalNumber);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameManager.instance.SceneToLoad(warpPortalNumber);
            }
        }
    }
}
