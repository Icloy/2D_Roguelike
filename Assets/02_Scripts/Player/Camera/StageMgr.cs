using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMgr : MonoBehaviour
{

    public GameObject Stage_Map1;
    public GameObject Stage_Map2;
    public GameObject Stage_Map3;

    public static StageMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageMgr>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("StageMgr");
                    instance = instanceContainer.AddComponent<StageMgr>();
                }
            }
            return instance;
        }
    }

    private static StageMgr instance;

    public Image FadeInOutImg;
    public Image WhitePanelImg;

    public List<GameObject> Stages = new List<GameObject>();
    public int currentStage = 0;

    float a;
    public IEnumerator FadeIn()
    {
        a = 1;
        FadeInOutImg.color = new Vector4(0, 0, 0, a);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator FadeIn1()
    {
        a = 0.4f;
        WhitePanelImg.color = new Vector4(255, 255, 255, a);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator FadeOut()
    {
        while (a >= 0)
        {
            FadeInOutImg.color = new Vector4(0, 0, 0, a);
            a -= 0.01f;
            yield return null;
        }
    }

    public IEnumerator FadeOut1()
    {
        while (a >= 0)
        {
            WhitePanelImg.color = new Vector4(255, 255, 255, a);
            a -= 0.005f;
            yield return null;
        }
    }

    public IEnumerator MoveNext (Collider2D collision, Vector3 destination, bool fadeInOut, bool SmoothMoving)
    {
        yield return null;
        if (fadeInOut)
        {
            yield return StartCoroutine(FadeIn());
        }
        mainCamera.Instance.cameraSmoothMoving = SmoothMoving;
        Stages[currentStage++].SetActive(false);
        Stages[currentStage].SetActive(true);
        switch (currentStage)
        {
            case 1:
                Stage_Map1.gameObject.SetActive(false);
                Stage_Map2.gameObject.SetActive(true);
                break;
            case 2:
                Stage_Map2.gameObject.SetActive(false);
                Stage_Map3.gameObject.SetActive(true);
                break;
        }
        collision.transform.position = destination;

        if (fadeInOut)
        {
            yield return StartCoroutine(FadeOut());
        }

      
    }

    public IEnumerator MoveNext2(Collider2D collision, Vector3 destination, bool fadeInOut, bool SmoothMoving)
    {
        yield return null;
        if (fadeInOut)
        {
            yield return StartCoroutine(FadeIn());
        }
        mainCamera.Instance.cameraSmoothMoving = SmoothMoving;


        collision.transform.position = destination;

        if (fadeInOut)
        {
            yield return StartCoroutine(FadeOut());
        }


    }


    public IEnumerator MoveNext3(bool fadeInOut, bool SmoothMoving) //
    {
        yield return null;
        if (fadeInOut)
        {
            yield return StartCoroutine(FadeIn1());
        }
        mainCamera.Instance.cameraSmoothMoving = SmoothMoving;


        if (fadeInOut)
        {
            yield return StartCoroutine(FadeOut1());
        }


    }
}
