using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendCarPropertiesToNetwork : MonoBehaviour
{
    [SerializeField] Transform flWheelHolder, frWheelHolder;

    public CarAnimation carAnimation => GetComponent<CarAnimation>();

    private void Update()
    {
        SendPropertiesToNetwork();
    }

    private void SendPropertiesToNetwork()
    {
        //NetworkSender.instance?.SendCarProperties(transform.position, transform.rotation,carAnimation.GetWheelSpeed,
        //                                                flWheelHolder.localRotation, frWheelHolder.localRotation);
        try
        {
            SendPackets.SendCarProperties(transform.position, transform.rotation, carAnimation.GetWheelSpeed, flWheelHolder.localRotation, frWheelHolder.localRotation);
        }
        catch (System.Exception)
        {
        }
    }
}