using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
 
    private void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(6, 4), CursorMode.Auto);
    }
}
