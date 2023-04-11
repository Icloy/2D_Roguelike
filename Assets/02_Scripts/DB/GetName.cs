using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetName : MonoBehaviour
{
    string getSaveDataURL = "http://localhost/SaveData/getname.php?";
    String date;

     public IEnumerator Getname(string name, Action<string> callback)
    {
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
        }
    }

   

    /*
       IEnumerator GetDB(string name)
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
               }
               else
               {
                   date = "null";
               }
           }
       }*/
}
