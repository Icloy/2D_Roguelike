using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IAPManager : MonoBehaviour
{
    private string API_KEY = "07583d99260273184f10e5c79b4e7031";
    private string URL = "https://kapi.kakao.com/v1/payment/ready";

    public IEnumerator Pay(int amount)
    {
        WWWForm form = new WWWForm();
        form.AddField("cid", "TC0ONETIME");
        form.AddField("partner_order_id", "partner_order_id");
        form.AddField("partner_user_id", "partner_user_id");
        form.AddField("item_name", "item_name");
        form.AddField("quantity", 1);
        form.AddField("total_amount", amount);
        form.AddField("tax_free_amount", 0);
        form.AddField("approval_url", "https://your.approval.url");
        form.AddField("cancel_url", "https://your.cancel.url");
        form.AddField("fail_url", "https://your.fail.url");

        UnityWebRequest request = UnityWebRequest.Post(URL, form);
        request.SetRequestHeader("Authorization", "KakaoAK " + API_KEY);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("KakaoPay Request Success: " + request.downloadHandler.text);
            // 카카오페이 결제 페이지로 이동합니다.
            Application.OpenURL(JsonUtility.FromJson<KakaoPayResponse>(request.downloadHandler.text).next_redirect_mobile_url);
        }
        else
        {
            Debug.Log("KakaoPay Request Failed: " + request.error);
        }
    }
}

[System.Serializable]
public class KakaoPayResponse
{
    public string tid;
    public string next_redirect_mobile_url;
}
