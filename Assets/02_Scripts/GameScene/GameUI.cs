using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Texture2D cursor;    //커서 이미지
    public CursorMode cm = CursorMode.Auto; //커서의 작동 방식
    public Vector2 cpos = Vector2.zero; //마우스 포인트의 위치

    private void Start()
    {
        Cursor.SetCursor(cursor, cpos, cm);
    }   
}
