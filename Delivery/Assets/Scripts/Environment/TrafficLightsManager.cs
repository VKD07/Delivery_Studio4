using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsManager : MonoBehaviour
{
    [SerializeField] TrafficLight[] trafficLights;
    [SerializeField] float warningLightTimeInterval = 1f;
    [SerializeField] float trafficLightsTimeInterval = 5f;

    #region private vars
    int currentTLindex;
    #endregion
    private void Start()
    {
        StartCoroutine(SetTrafficLights());
        trafficLights[currentTLindex].SetRedLight(false, warningLightTimeInterval);
    }

    IEnumerator SetTrafficLights()
    {
        while (true)
        {
            yield return new WaitForSeconds(trafficLightsTimeInterval);
            trafficLights[currentTLindex].SetRedLight(true, warningLightTimeInterval);

            yield return new WaitForSeconds(trafficLightsTimeInterval);
            ManageCurrentTLIndex();
            trafficLights[currentTLindex].SetRedLight(false, warningLightTimeInterval);
        }
    }

    void ManageCurrentTLIndex()
    {
        if (currentTLindex < trafficLights.Length -1)
        {
            currentTLindex++;
        }
        else
        {
            currentTLindex = 0;
        }
    }
}
