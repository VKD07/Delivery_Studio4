using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Client.instance?.SendPacket(new DriverCollidedPacket(true).Serialize());
        Debug.Log("Driver has collided something");
    }
}
