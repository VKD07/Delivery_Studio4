using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script is responsible for handling the chosen delivery location
/// </summary>
public class LocationHandler : MonoBehaviour
{
    public static LocationHandler instance;

    #region Private Vars
    Client client;
    #endregion
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this);
        }
        client = Client.instance;
    }

    private void OnEnable()
    {
        if (client == null) return;
        client.onDeliveryAddress += SetDeliveryLocation;
    }

    private void OnDisable()
    {
        if (client == null) return;
        client.onDeliveryAddress -= SetDeliveryLocation;
    }

    [SerializeField] GameObject[] deliveryLocations;

    //private variables
    GameObject chosenLocation;
    bool addressAvailable;

    void Update()
    {
        EnableDeliveryLocation();
    }

    private void EnableDeliveryLocation()
    {
        if (!addressAvailable && chosenLocation != null)
        {
            addressAvailable = true;
            chosenLocation.transform.Find("GlowRing").gameObject.SetActive(true);
            Debug.Log($"Delivery Location Set {chosenLocation.name}");
        }    
    }

    #region Setters
    public void SetDeliveryLocation(DeliveryLocationPacket packet)
    {
        Debug.Log(packet.buildingName);
        for (int i = 0; i < deliveryLocations.Length; i++)
        {
            if(deliveryLocations[i].name == packet.buildingName)
            {
                chosenLocation = deliveryLocations[i];
                Debug.Log("location found");
                break;
            }
        }
    }
    #endregion
}