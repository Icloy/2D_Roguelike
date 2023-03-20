using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlowMotionClass : MonoBehaviour
{
    public static SlowMotionClass instance { get; protected set; }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public float slowFactor = 0.05f;
    public float slowLength = 4f;

    //public Action OnSlowMotionAudio;

    public void DoSlowMotion()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void Update()
    {
        Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        //OnSlowMotionAudio.Invoke();
    }
}
