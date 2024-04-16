using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeliveryLocationSetter : MonoBehaviour
{

    [SerializeField] GameObject[] deliveryLocations;
    [SerializeField] string glowRingName = "GlowRing";
    [SerializeField] float choosingLocationDelayTime = 5f;
    int chosenIndex;

    [Header("=== EFFECT SETTINGS ===")]
    [SerializeField] Color colorIndicator;
    [SerializeField] float fadeSpeed;

    [Header("=== EVENTS ===")]
    [SerializeField] UnityEvent onLocationSelected;
    bool hasChosen;

    #region Getters
    public string chosenBuilding { get; private set; }
    #endregion

    #region Building Components
    Material buildingMat;
    Color initColor;
    #endregion

    private void Awake()
    {
        StartCoroutine(ChooseDeliveryLocation());
    }

    private void Update()
    {
        if (chosenBuilding == null) return;
        if (!hasChosen)
        {
            hasChosen = true;
            EnableBuildingGlowRing();
            StartCoroutine(LerpBuildingColor());

            StartCoroutine(SendDeliveryLocationToDriverPartner());
        }
    }

    IEnumerator SendDeliveryLocationToDriverPartner()
    {
        yield return new WaitForSeconds(choosingLocationDelayTime + 2);
        //NetworkSender.instance?.SendDeliveryLocationToDriver(chosenBuilding);
        SendPackets.SendDeliveryLocation(chosenBuilding);
    }

    IEnumerator ChooseDeliveryLocation()
    {
        yield return new WaitForSeconds(choosingLocationDelayTime);
        chosenIndex = Random.Range(0, deliveryLocations.Length);
        chosenBuilding = deliveryLocations[chosenIndex].name;
        onLocationSelected.Invoke();
        //sending the building name to the network
    }

    void EnableBuildingGlowRing()
    {
        deliveryLocations[chosenIndex].transform.Find(glowRingName).gameObject.SetActive(true);
    }

    IEnumerator LerpBuildingColor()
    {
        buildingMat = deliveryLocations[chosenIndex].GetComponent<MeshRenderer>().material;
        initColor = buildingMat.GetColor("_BaseColor");

        while (true)
        {
            yield return null;
            buildingMat.SetColor("_BaseColor", Color.Lerp(initColor, colorIndicator, Mathf.Sin(fadeSpeed * Time.time)));
            //buildingMat.color = Color.Lerp(initColor, colorIndicator, Mathf.Sin(fadeSpeed * Time.time));
        }
    }
}
