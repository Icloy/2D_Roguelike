using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSake : MonoBehaviour
{
    public static CSake instance;

    public float vibrationDuration = 0.25f;
    public float vibrationIntensity = 1f;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;



    void Start()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void Vibrate(float vibrationIntensity)
    {
        CinemachineImpulseSource impulseSource = GetComponent<CinemachineImpulseSource>();
        impulseSource.GenerateImpulse();

        noise.m_AmplitudeGain = vibrationIntensity;
        noise.m_FrequencyGain = vibrationIntensity;

        Invoke("StopVibrating", vibrationDuration);
    }

    void StopVibrating()
    {
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

}
