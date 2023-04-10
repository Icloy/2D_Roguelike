using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class SaveData : MonoBehaviour
{
    string secretKey = "mySecretKey";
    public string saveDataURL = "http://localhost/SaveData/senddata.php?";

    public void SendScoreBtn()
    {
        System.DateTime now = System.DateTime.Now;
        string dateString = now.ToString("yyyy-MM-dd HH:mm");
        StartCoroutine(PostScores("slot0", dateString, GameManager.instance.coin, Player.instance.curHp,Player.instance.maxHp,Player.instance.AtDmg));
    }

    IEnumerator PostScores(string name, string date,int coin, int curhp, int maxhp, int dmg)
    {
        string hash = HashInput(name + coin + curhp + maxhp + dmg + secretKey);
        string post_url = saveDataURL + "name=" + UnityWebRequest.EscapeURL(name) + "&date=" + date + "&coin=" + coin + "&curhp=" + curhp + "&maxhp=" + maxhp + "&dmg=" + dmg + "&hash=" + hash;
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");
        hs_post.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(hash));
        hs_post.SetRequestHeader("Content-Type", "application/json");
        yield return hs_post.SendWebRequest();
        if (hs_post.error != null)
            Debug.Log("There was an error posting the SaveData: " + hs_post.error);
    }

    public string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }
}
