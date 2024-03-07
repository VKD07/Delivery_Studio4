using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        using(DriverCollidedPacket packet = new DriverCollidedPacket(true, Client.instance.playerData))
        {
            Client.instance?.SendPacket(packet.Serialize());
        }
        Debug.Log("Driver has collided something");
    }
}