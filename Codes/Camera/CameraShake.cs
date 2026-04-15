using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoSingleton<CameraShake>
{
    private CinemachineVirtualCamera virtualC;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        virtualC = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        noise = virtualC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCameraSmall()
    {
        noise.m_FrequencyGain = 0.15f;
    }

    public void ShakeCameraMiddle()
    {
        noise.m_FrequencyGain = 0.2f;
    }

    public void ShakeCameraLarge()
    {
        noise.m_FrequencyGain = 0.25f;
    }

    public void BigShake()
    {
        noise.m_AmplitudeGain = 8f;
    }

    public void StopShake()
    {
        noise.m_AmplitudeGain = 1.3f;
        noise.m_FrequencyGain = 0f;
    }
}

