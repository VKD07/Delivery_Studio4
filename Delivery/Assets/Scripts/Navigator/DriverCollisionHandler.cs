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


    //TODO: When collided, the map will go next randomly

    public void TriggerRandomMapRotation(bool value)
    {
        if (!value) return;
        OnDriverCollided.Invoke();
    }
}
