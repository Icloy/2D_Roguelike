using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
     private CanvasGroup cg;
    public float fadeTime = 1f; // 페이드 타임 
    float accumTime = 0f;
    private Coroutine fadeCor;
    public GameObject BossImg;

   private void Awake()
    {
        cg = gameObject.GetComponent<CanvasGroup>(); // 캔버스 그룹
        StartFadeIn();
        Invoke("OffImg", 5.5f);
    }

   public void StartFadeIn() // 호출 함수 Fade In을 시작
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());
    }

   private IEnumerator FadeIn() // 코루틴을 통해 페이드 인 시간 조절
    {
        yield return new WaitForSeconds(0.2f);
        accumTime = 0f;
        while (accumTime < fadeTime) 
        {
            cg.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 1f;

        StartCoroutine(FadeOut()); //일정시간 켜졌다 꺼지도록 Fade out 코루틴 호출

    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3.0f);
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 0f;
    }

    private void OffImg()
    {
        cg.enabled = false;
        BossImg.SetActive(false);
    }


}
