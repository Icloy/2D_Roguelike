using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Map : MonoBehaviour
{

    public Button[] TP;

    public GameObject[] Tppoint;
    public GameObject TpPoint;

    public GameObject PopupPanel; // 팝업 패널

    private int num; 

    private void Awake()
    {
        TP = GetComponentsInChildren<Button>().Where(b => b.gameObject.name.Contains("TP")).ToArray();
    }

    private void Start()
    {
        Tppoint = new GameObject[TP.Length];
        for (int i = 0; i < TP.Length; i++)
        {
            Tppoint[i] = TpPoint.transform.GetChild(i).gameObject;
        }
    }

    public void Teleport(int tpPoint) //TP버튼 클릭시
    {
        PopupPanel.SetActive(true);
        num = tpPoint;
    }

    public void PopupY() //팝업 확인창
    {
        //플레이어 이동
        Player.instance.transform.position = Tppoint[num].transform.position;
        PopupPanel.SetActive(false);
    }

    public void PopupN() //팝업 취소창
    {
        PopupPanel.SetActive(false);
    }
}
