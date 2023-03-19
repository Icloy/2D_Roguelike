using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TpPoint : MonoBehaviour
{
    public Button btn;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            ToastMsg.Instance.showMessage("����Ʈ�� ����Ͻ÷��� G�� �����ּ���!", 1f);
            StartCoroutine("Register");
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StopCoroutine("Register");
        }
    }

    IEnumerator Register()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.G))
            {
                //TP Ȱ��ȭ
                btn.interactable = true;
                yield return null;
            }
        }
    }
}
