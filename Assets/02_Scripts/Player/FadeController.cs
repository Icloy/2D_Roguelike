using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
     private CanvasGroup cg;
    public float fadeTime = 1f; // ���̵� Ÿ�� 
    float accumTime = 0f;
    private Coroutine fadeCor;
    public GameObject BossImg;

   private void Awake()
    {
        cg = gameObject.GetComponent<CanvasGroup>(); // ĵ���� �׷�
        StartFadeIn();
        Invoke("OffImg", 5.5f);
    }

   public void StartFadeIn() // ȣ�� �Լ� Fade In�� ����
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());
    }

   private IEnumerator FadeIn() // �ڷ�ƾ�� ���� ���̵� �� �ð� ����
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

        StartCoroutine(FadeOut()); //�����ð� ������ �������� Fade out �ڷ�ƾ ȣ��

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
