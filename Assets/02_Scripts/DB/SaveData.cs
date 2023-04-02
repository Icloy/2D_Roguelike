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
        StartCoroutine(PostScores("lys", GameManager.instance.coin, Player.instance.curHp,Player.instance.maxHp,Player.instance.AtDmg));
    }

    IEnumerator PostScores(string name, int coin, int curhp, int maxhp, int dmg)
    {
        string hash = HashInput(name + coin + curhp + maxhp + dmg + secretKey);
        string post_url = saveDataURL + "name=" + UnityWebRequest.EscapeURL(name) + "&coin=" + coin + "&curhp=" + curhp + "&maxhp=" + maxhp + "&dmg=" + dmg + "&hash=" + hash;
        UnityWebRequest hs_post = UnityWebRequest.Post(post_url, "");
        hs_post.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(hash));
        hs_post.SetRequestHeader("Content-Type", "application/json");
        yield return hs_post.SendWebRequest();
        if (hs_post.error != null)
            Debug.Log("There was an error posting the high score: " + hs_post.error);
    }

    public string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }
}
