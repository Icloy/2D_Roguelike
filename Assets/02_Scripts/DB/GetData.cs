using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GetData : MonoBehaviour
{
    string getSaveDataURL = "http://localhost/SaveData/getdata.php?";
    int coin, curhp, maxhp, dmg;

    public void GetDbData()
    {
        StartCoroutine(GetDB("lys"));
    }

    IEnumerator GetDB(string name)
    {
        UnityWebRequest www = UnityWebRequest.Get(getSaveDataURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;

            // JSON ������ �����͸� Dictionary�� ��ȯ
            Dictionary<string, object> data = JsonUtility.FromJson<Dictionary<string, object>>(jsonString);

            // �� �����͸� ������ ����
            string name1 = (string)data["name"];
            int coin = (int)(long)data["coin"]; // coin�� int�� ������������, JSON���κ��� long������ �о���� ������ ����ȯ �ʿ�
            int curhp = (int)(long)data["curhp"];
            int maxhp = (int)(long)data["maxhp"];
            int dmg = (int)(long)data["dmg"];

            // ������ ���
            Debug.Log("Name: " + name1);
            Debug.Log("Coin: " + coin);
            Debug.Log("Current HP: " + curhp);
            Debug.Log("Max HP: " + maxhp);
            Debug.Log("Damage: " + dmg);
        }
    }
}