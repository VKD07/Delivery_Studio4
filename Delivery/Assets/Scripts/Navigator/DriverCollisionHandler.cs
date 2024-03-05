using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DriverCollisionHandler : MonoBehaviour
{
    public static DriverCollisionHandler instance;

    public UnityEvent OnDriverCollided;

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

    private void OnEnable()
    {
        Client.instance.onDriverCollision += TriggerRandomMapRotation;
    }
    private void OnDisable()
    {
        Client.instance.onDriverCollision -= TriggerRandomMapRotation;
    }

    //TODO: When collided, the map will go next randomly

    public void TriggerRandomMapRotation(DriverCollidedPacket packet)
    {
        if (packet.hasCollided)
        {
            OnDriverCollided.Invoke();
        }
    }
}
