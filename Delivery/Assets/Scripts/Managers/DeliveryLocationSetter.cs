using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLocationSetter : MonoBehaviour
{

    [SerializeField] GameObject[] deliveryLocations;
    [SerializeField] string glowRingName = "GlowRing";
    int chosenIndex;

    [Header("=== EFFECT SETTINGS ===")]
    [SerializeField] Color colorIndicator;
    [SerializeField] float fadeSpeed;

    #region Getters
    public string chosenBuilding { get; private set; }
    #endregion

    #region Building Components
    Material buildingMat;
    Color initColor;
    #endregion

    private void Awake()
    {
        ChooseDeliveryLocation();
    }

    private void Start()
    {
        if (chosenBuilding == null) return;
        EnableBuildingGlowRing();
        StartCoroutine(LerpBuildingColor());

        StartCoroutine(SendDeliveryLocationToDriverPartner());
    }

    IEnumerator SendDeliveryLocationToDriverPartner()
    {
        yield return new WaitForSeconds(2);
        //NetworkSender.instance?.SendDeliveryLocationToDriver(chosenBuilding);
        SendPackets.SendDeliveryLocation(chosenBuilding);
    }

    void ChooseDeliveryLocation()
    {
        chosenIndex = Random.Range(0, deliveryLocations.Length);
        chosenBuilding = deliveryLocations[chosenIndex].name;
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
