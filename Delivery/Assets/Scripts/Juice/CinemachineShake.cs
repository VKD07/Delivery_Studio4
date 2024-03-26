using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get; private set; }
    CinemachineVirtualCamera vc => GetComponent<CinemachineVirtualCamera>();
    CinemachineBasicMultiChannelPerlin mcp;
    float shakeTimer;
    float shakeTimerTotal;
    float startingIntensity;

    public bool loop;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }


    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                mcp.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));

                if (loop)
                {
                    shakeTimer = shakeTimerTotal;
                    mcp.m_AmplitudeGain = Mathf.Lerp(mcp.m_AmplitudeGain, startingIntensity, Time.deltaTime * .1f);
                }
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        mcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        startingIntensity = intensity;
        mcp.m_AmplitudeGain = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}
