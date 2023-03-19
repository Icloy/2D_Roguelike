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
            ToastMsg.Instance.showMessage("포인트를 등록하시려면 G를 눌러주세요!", 1f);
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
                //TP 활성화
                btn.interactable = true;
                yield return null;
            }
        }
    }
}
