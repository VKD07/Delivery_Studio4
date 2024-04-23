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
        SendCollisionData();
        //NetworkSender.instance?.SendCollisionPacket();
        CinemachineShake.instance?.ShakeCamera(shakeIntensity, shakeTimer);
        SendPackets.SendDriverCollision();
    }

    void SendCollisionData()
    {
        switch (MapChooser.instance?.chosenSetOfMapID)
        {
            case 0:
                //Collecting data
                try
                {
                    CollectData.instance.kioMapCrashCount++;
                }
                catch (System.Exception)
                {
                    Debug.Log("Collect Data Not found");
                }
                break;

            case 1:
                //Collecting data
                try
                {
                    CollectData.instance.cityMapCrashCount++;
                }
                catch (System.Exception)
                {
                    Debug.Log("Collect Data Not found");
                }
                break;
        }

    }
}
