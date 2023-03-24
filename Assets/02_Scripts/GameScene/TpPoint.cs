using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TpPoint : MonoBehaviour
{
    public Button btn;

    private bool isRegister = false;

    GameObject flash;

    private void Awake()
    {
        flash = this.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !isRegister)
        {
            ToastMsg.Instance.showMessage("����Ʈ�� ����Ͻ÷��� G�� �����ּ���!", 1f);
            StartCoroutine("Register");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
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
                ToastMsg.Instance.showMessage("����Ͽ����ϴ�.!", 1f);
                flash.SetActive(true);
                isRegister = true;
                break;
            }
            yield return null;
        }
    }
}
