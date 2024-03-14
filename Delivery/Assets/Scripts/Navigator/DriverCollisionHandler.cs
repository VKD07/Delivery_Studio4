using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DriverCollisionHandler : MonoBehaviour
{
    public static DriverCollisionHandler instance;

    public UnityEvent OnDriverCollided;
    public UnityEvent OnDriverCollidedOnDirt;

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

    public void TriggerRandomMapRotation()
    {
        OnDriverCollided.Invoke();
    }

    public void EnableDirtScreen()
    {
        OnDriverCollidedOnDirt.Invoke();
    }
}
