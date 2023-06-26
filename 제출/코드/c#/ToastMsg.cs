using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToastMsg : MonoBehaviour
{
    public TextMeshProUGUI toast;

    static ToastMsg instance = null;

    float fadeInOutTime = 0.3f;

    public static ToastMsg Instance
    {
        get
        {
            if (null == instance) instance = FindObjectOfType<ToastMsg>();
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance) instance = this;
    }

    struct TOAST
    {
        public string msg;
        public float durationTime;
    }

    Queue<TOAST> toastQueue = new Queue<TOAST>();
    bool isPopUp; //토스트 메시지가 실행중인지 여부

    public void showMessage(string msg, float durationTime)
    {
        TOAST t;

        t.msg = msg;
        t.durationTime = durationTime;

        toastQueue.Enqueue(t);
        if(isPopUp == true) //실행중이라면 이전까지의 토스트메시지 삭제
        {
            StopAllCoroutines();
            isPopUp = false;
        }

        if (isPopUp == false)
        {
            StartCoroutine(showToastQueue());
        }
    }

    IEnumerator showToastQueue()
    {
        isPopUp = true;

        while (toastQueue.Count != 0)
        {
            TOAST t = toastQueue.Dequeue();
            yield return StartCoroutine(showMessageCoroutine(t.msg, t.durationTime));
        }

        isPopUp = false;
    }

    IEnumerator showMessageCoroutine(string msg, float durationTime)
    {
        toast.text = msg;
        toast.enabled = true;

        yield return fadeInOut(toast, fadeInOutTime, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return fadeInOut(toast, fadeInOutTime, false);

        toast.enabled = false;
    }

    IEnumerator fadeInOut(TextMeshProUGUI target, float durationTime, bool inOut)
    {
        float start, end;
        if (inOut)
        {
            start = 0.0f;
            end = 1.0f;
        }
        else
        {
            start = 1.0f;
            end = 0f;
        }

        Color current = Color.clear; 
        
        //색지정 현재=노란색
        current.r = 255;
        current.g = 255;
        
        float elapsedTime = 0.0f;

        while (elapsedTime < durationTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / durationTime);

            target.color = new Color(current.r, current.g, current.b, alpha);


            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
