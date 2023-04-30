using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    float minIntensity = 0f;
    float maxIntensity = 1.0f;
    float speed = 0.1f;

    private UnityEngine.Rendering.Universal.Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }
    private void Update()
    {
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
