using Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class SendCarAnimation : MonoBehaviour
{
    [SerializeField] Transform[] frontWheelHolders;

    #region private vars
    #endregion

    #region Required Components
    public CarController carController => GetComponent<CarController>();
    public CarAnimation carAnimation => GetComponent<CarAnimation>();
    #endregion

    private void Start()
    {
    }

    /// <summary>
    /// Fix This
    /// </summary>
    private void LateUpdate()
    {
        SendFrontWheelHolderRotation();
    }

    private void FixedUpdate()
    {
    }

    private void SendFrontWheelHolderRotation()
    {
        for (int i = 0; i < frontWheelHolders.Length; i++)
        {
            Client.instance?.SendPacket(new FrontWheelHolderPacket(frontWheelHolders[i].localRotation));
        }
    }

    private void SendWheelRotation()
    {
        Client.instance.SendPacket(new WheelRotationPacket(carAnimation.GetWheelSpeed));
    }

}
