using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Unity.Mathematics;

public class GetName : MonoBehaviour
{
    string getSaveDataURL = "http://localhost/SaveData/getname.php?";
    public TMP_Text[] slot;

    String date;


    public void Refresh()
    {
        foreach (TMP_Text item in slot)
        {
            StartCoroutine(Getname(item.name, result =>
            {
                date = result;

                if (date == null)
                {

                    item.text = "플레이 기록 없음!";
                }
                else
                {
                    string dateTimeString = date;
                    DateTime dateTime = DateTime.Parse(dateTimeString);
                    date = dateTime.ToString("yyyy-MM-dd HH:mm");
                    item.text = "마지막 플레이 : " + date;
                }
            }));
        }
    }



    IEnumerator Getname(string name, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);

        UnityWebRequest www = UnityWebRequest.Post(getSaveDataURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error retrieving data: " + www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            JSONNode jsonNode = JSON.Parse(jsonString);

            if (jsonNode != null && jsonNode[0].Tag == JSONNodeType.Object)
            {
                JSONObject jsonObject = jsonNode[0].AsObject;
                date = jsonObject["date"];
                callback(date);
            }
            else
            {
                date = "null";
                callback(null);
            }
        }











        /*//Debug.Log("Getname에 들어가는 이름 : " + name);
        UnityWebRequest www = UnityWebRequest.Post(getSaveDataURL + "name=" + UnityWebRequest.EscapeURL(name), "");
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error retrieving data: " + www.error);
            callback(null);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            JSONNode jsonNode = JSON.Parse(jsonString);
            Debug.Log(jsonNode + "dd");
            if (jsonNode != null && jsonNode[0].Tag == JSONNodeType.Object)
            {
                JSONObject jsonObject = jsonNode[0].AsObject;

                date = jsonObject["date"];
                callback(date);
            }
            else
            {
                date = "null";
                callback(null);
            }
        }*/
    }
}
