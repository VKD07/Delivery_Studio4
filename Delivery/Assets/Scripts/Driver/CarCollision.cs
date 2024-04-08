using Rubickanov.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarCollision : MonoBehaviour
{
    public UnityEvent OnCarCollided;

    [Header("=== CAMERA SHAKE SETTINGS ===")]
    public float shakeIntensity;
    public float shakeTimer;
    private void OnCollisionEnter(Collision collision)
    {
        OnCarCollided.Invoke();
        //NetworkSender.instance?.SendCollisionPacket();
        CinemachineShake.instance?.ShakeCamera(shakeIntensity, shakeTimer);
        SendPackets.SendDriverCollision();
    }
}
