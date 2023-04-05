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

            // JSON 형식의 데이터를 Dictionary로 변환
            Dictionary<string, object> data = JsonUtility.FromJson<Dictionary<string, object>>(jsonString);

            // 각 데이터를 변수에 저장
            string name1 = (string)data["name"];
            int coin = (int)(long)data["coin"]; // coin은 int형 데이터이지만, JSON으로부터 long형으로 읽어오기 때문에 형변환 필요
            int curhp = (int)(long)data["curhp"];
            int maxhp = (int)(long)data["maxhp"];
            int dmg = (int)(long)data["dmg"];

            // 데이터 출력
            Debug.Log("Name: " + name1);
            Debug.Log("Coin: " + coin);
            Debug.Log("Current HP: " + curhp);
            Debug.Log("Max HP: " + maxhp);
            Debug.Log("Damage: " + dmg);
        }
    }
}