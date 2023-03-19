using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TpPoint : MonoBehaviour
{
    public Button btn;

    private bool isRegister = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !isRegister)
        {
            ToastMsg.Instance.showMessage("포인트를 등록하시려면 G를 눌러주세요!", 1f);
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
                //TP 활성화
                btn.interactable = true;
                ToastMsg.Instance.showMessage("등록하였습니다.!", 1f);
                isRegister = true;
                break;
            }
            yield return null;
        }
    }
}
