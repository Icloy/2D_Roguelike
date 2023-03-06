using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Texture2D cursor;    //Ŀ�� �̹���
    public CursorMode cm = CursorMode.Auto; //Ŀ���� �۵� ���
    public Vector2 cpos = Vector2.zero; //���콺 ����Ʈ�� ��ġ

    private void Start()
    {
        Cursor.SetCursor(cursor, cpos, cm);
    }   
}
