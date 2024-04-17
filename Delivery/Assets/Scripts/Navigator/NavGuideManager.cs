using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavGuideManager : MonoBehaviour
{
    [Header("Map Guide")]
    [SerializeField] GameObject mapGuide;
    [SerializeField] float timeToDisable = 5f;

    [Header("Car Manual Guide")]
    [SerializeField] GameObject carManualMain;
    [SerializeField] GameObject manualGuide;
    [SerializeField] float disableTime = 5f;
    bool manualGuideDisabled;
    void Start()
    {
        mapGuide.SetActive(true);
        StartCoroutine(DisableMapGuide());
    }

    private void Update()
    {
        if (carManualMain.activeSelf && !manualGuideDisabled)
        {
            manualGuideDisabled = true;
            manualGuide.SetActive(true);
            StartCoroutine(DisableManualGuide());
        }
    }

    IEnumerator DisableMapGuide()
    {
        yield return new WaitForSeconds(timeToDisable);
        mapGuide.SetActive(false);
    }

    IEnumerator DisableManualGuide()
    {
        yield return new WaitForSeconds(disableTime);
        manualGuide.SetActive(false);
    }
}
