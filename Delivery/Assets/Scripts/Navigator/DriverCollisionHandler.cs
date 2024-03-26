using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DriverCollisionHandler : MonoBehaviour
{
    public static DriverCollisionHandler instance;

    [Header("=== CAMERA SHAKE SETTINGS ===")]
    [SerializeField] float shakeIntensity = .3f;
    [SerializeField] float shakeTime = .3f;

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
        CinemachineShake.instance?.ShakeCamera(shakeIntensity, shakeTime);
        OnDriverCollided.Invoke();
    }

    public void EnableDirtScreen()
    {
        OnDriverCollidedOnDirt.Invoke();
    }
}
