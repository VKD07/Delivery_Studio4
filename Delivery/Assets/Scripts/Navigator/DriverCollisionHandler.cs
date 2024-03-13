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

    public void TriggerRandomMapRotation(bool hasCollided, PlayerData playerData)
    {
        if (playerData.teamNumber != Client.instance.playerData.teamNumber) { return; }

        if (hasCollided)
        {
            OnDriverCollided.Invoke();
        }
    }

    public void EnableDirtScreen(PlayerData playerData)
    {
        if (playerData.teamNumber != Client.instance.playerData.teamNumber) { return; }
        OnDriverCollidedOnDirt.Invoke();
    }

    public void TriggerRandomMapRotation()
    {
        OnDriverCollided.Invoke();
    }
}
