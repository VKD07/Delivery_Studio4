using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMalfunction : MonoBehaviour
{
    [SerializeField] int numberOfCrashesToTrigger = 4;
    int crashCount;

    void TriggerCarMalfunction()
    {
        if(crashCount < numberOfCrashesToTrigger)
        {
            crashCount++;
        }
        else
        {
            crashCount = 0;
        }
    }
}
