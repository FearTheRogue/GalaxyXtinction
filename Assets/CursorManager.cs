using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [SerializeField] private Texture2D currentCursor;
    [SerializeField] private Texture2D previousCursor;

    [SerializeField] private Texture2D[] cursors;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ActivateNormalCursor()
    {
        if(currentCursor != null)
        {
            previousCursor = currentCursor;
        }

        currentCursor = cursors[0];

        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursors[0], new Vector2(6, 4), CursorMode.Auto);
    }

    public void ActivateCrosshairCursor()
    {
        if (currentCursor != null)
        {
            previousCursor = currentCursor;
        }

        currentCursor = cursors[1];

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(cursors[1], new Vector2(64, 65), CursorMode.Auto);
    }

    public void ActivateCenter()
    {
        if (currentCursor != null)
        {
            previousCursor = currentCursor;
        }

        currentCursor = cursors[2];

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.SetCursor(cursors[2], new Vector2(14, 14), CursorMode.Auto);
    }

    public void ActivatePreviousCursor()
    {
        if (currentCursor != null)
        {
            Texture2D temp = previousCursor;

            previousCursor = currentCursor;

            currentCursor = temp;
        }
    }
}