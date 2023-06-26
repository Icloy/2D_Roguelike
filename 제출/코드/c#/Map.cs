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

    public GameObject PopupPanel; // �˾� �г�

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

    public void Teleport(int tpPoint) //TP��ư Ŭ����
    {
        PopupPanel.SetActive(true);
        num = tpPoint;
    }

    public void PopupY() //�˾� Ȯ��â
    {
        //�÷��̾� �̵�
        Player.instance.transform.position = Tppoint[num].transform.position;
        PopupPanel.SetActive(false);
    }

    public void PopupN() //�˾� ���â
    {
        PopupPanel.SetActive(false);
    }
}
