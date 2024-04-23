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
    [SerializeField] GameObject glowRing;

    #region Private Vars
    ClientManager client;
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
        client = ClientManager.instance;
    }

    //private variables
    GameObject chosenLocation;
    bool addressAvailable;

    void Update()
    {
       // EnableDeliveryLocation();
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

    #region Network Receivers
    public void SetDeliveryLocation(string buildingName, Vector3 glowRingPos, Quaternion glowRingRot)
    {
        chosenLocation = GameObject.Find(buildingName);
        GameObject location = Instantiate(glowRing);
        location.transform.parent = chosenLocation.transform;
        location.transform.localPosition = glowRingPos;
        location.transform.localRotation = glowRingRot;
        location.GetComponent<TargetLocation>().hideGlow = true;
        location.gameObject.SetActive(true);


        Debug.Log($"Delivery Name Received: {chosenLocation.name}");
        //for (int i = 0; i < deliveryLocations.Length; i++)
        //{
        //    if (deliveryLocations[i].name == buildingName)
        //    {
        //        chosenLocation = deliveryLocations[i];
        //        Debug.Log("location found");
        //        break;
        //    }
        //}
    }

    //public void SetDeliveryLocation(PlayerData playerData, string buildingName)
    //{
    //    if (playerData.teamNumber != client.playerData.teamNumber) return;

    //    Debug.Log(buildingName);
    //    for (int i = 0; i < deliveryLocations.Length; i++)
    //    {
    //        if(deliveryLocations[i].name == buildingName)
    //        {
    //            chosenLocation = deliveryLocations[i];
    //            Debug.Log("location found");
    //            break;
    //        }
    //    }
    //}
    #endregion
}