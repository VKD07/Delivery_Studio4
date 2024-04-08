using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarEngineShake : MonoBehaviour
{
    public float shakeIntensity, shakeTime;
    Rigidbody rb => GetComponent<Rigidbody>();
    CarMalfunction carMalfunction => GetComponent<CarMalfunction>();

    private void Update()
    {
        TriggerEngineShake();
    }

    private void TriggerEngineShake()
    {
        if (rb.velocity.magnitude < 0.5f && !carMalfunction.isBroken)
        {
            CinemachineShake.instance.loop = true;
            CinemachineShake.instance?.ShakeCamera(shakeIntensity, shakeTime);
        }
        else
        {
            CinemachineShake.instance.loop = false;
        }
    }

    public void TriggerCarCrankingShake(bool val)
    {
        CinemachineShake.instance.loop = true;
        CinemachineShake.instance?.ShakeCamera(shakeIntensity, shakeTime);
    }
}
