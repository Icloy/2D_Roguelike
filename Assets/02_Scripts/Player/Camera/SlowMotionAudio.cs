using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionAudio : MonoBehaviour
{
    private void OnEnable()
    {
        SlowMotionClass.instance.OnSlowMotionAudio += OnSlowMotionAudio;
    }

    private void OnDisable()
    {
        SlowMotionClass.instance.OnSlowMotionAudio -= OnSlowMotionAudio;
    }

    void OnSlowMotionAudio()
    {
        GetComponent<AudioSource>().pitch = Time.timeScale;
    }
}
