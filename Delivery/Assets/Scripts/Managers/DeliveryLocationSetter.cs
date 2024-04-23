using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DeliveryLocationSetter : MonoBehaviour
{
    public static DeliveryLocationSetter instance;
    [SerializeField] List<GameObject> deliveryLocations;
    [SerializeField] string glowRingName = "GlowRing";
    [SerializeField] float choosingLocationDelayTime = 5f;
    int chosenIndex;
    TargetLocation[] targetLocations;
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

    private void Start()
    {
        targetLocations = FindObjectsOfType<TargetLocation>(true);

        for (int i = 0; i < targetLocations.Length; i++)
        {
            deliveryLocations.Add(targetLocations[i].transform.parent.gameObject);
        }

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

    IEnumerator ChooseDeliveryLocation()
    {
        yield return new WaitForSeconds(choosingLocationDelayTime);
        chosenIndex = Random.Range(0, deliveryLocations.Count);
        chosenBuilding = deliveryLocations[chosenIndex].name;
        onLocationSelected.Invoke();
        //sending the building name to the network
    }


    void EnableBuildingGlowRing()
    {
        targetLocations[chosenIndex].transform.gameObject.SetActive(true);
    }

    IEnumerator SendDeliveryLocationToDriverPartner()
    {
        yield return new WaitForSeconds(choosingLocationDelayTime + 2);
        //NetworkSender.instance?.SendDeliveryLocationToDriver(chosenBuilding);
        SendPackets.SendDeliveryLocation(chosenBuilding, targetLocations[chosenIndex].transform.localPosition,
                                        targetLocations[chosenIndex].transform.localRotation);
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
