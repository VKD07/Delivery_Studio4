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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerRandomMapRotation();
        }
    }

    public void TriggerRandomMapRotation(bool hasCollided, PlayerData playerData)
    {
        if (playerData.teamNumber != Client.instance.playerData.teamNumber) { return; }

        if (hasCollided)
        {
            OnDriverCollided.Invoke();
        }
    }

    public void TriggerRandomMapRotation()
    {
        OnDriverCollided.Invoke();
    }
}
