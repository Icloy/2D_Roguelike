using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

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
            Debug.Log(jsonNode);

            if (jsonNode != null && jsonNode[0].Tag == JSONNodeType.Object)
            {
                JSONObject jsonObject = jsonNode[0].AsObject;

                coin = jsonObject["coin"].AsInt;
                curhp = jsonObject["curhp"].AsInt;
                maxhp = jsonObject["maxhp"].AsInt;
                dmg = jsonObject["dmg"].AsInt;

                Debug.Log("Coin: " + coin);
                Debug.Log("CurHP: " + curhp);
                Debug.Log("MaxHP: " + maxhp);
                Debug.Log("Dmg: " + dmg);
            }
            else
            {
                Debug.Log("No data retrieved.");
            }
        }
    }
}
