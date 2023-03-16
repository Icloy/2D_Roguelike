using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : MonoBehaviour
{
    public Texture2D cursor;    //Ŀ�� �̹���
    CursorMode cm = CursorMode.Auto; //Ŀ���� �۵� ���
    Vector2 cpos = Vector2.zero; //���콺 ����Ʈ�� ��ġ

    private void Start()
    {
        Cursor.SetCursor(cursor, cpos, cm);
    }
}
