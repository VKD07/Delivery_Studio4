using Rubickanov.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarCollision : MonoBehaviour
{
    public UnityEvent OnCarCollided;
    private void OnCollisionEnter(Collision collision)
    {
        OnCarCollided.Invoke();
        //NetworkSender.instance?.SendCollisionPacket();
        SendPackets.SendDriverCollision();
    }
}
