using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines;

public class TrafficAwareness : MonoBehaviour
{
    [Header("=== AWARENESS SETTINGS ===")]
    [SerializeField] Transform visionPoint;
    [SerializeField] float visionRadius = 5f;
    [SerializeField] float castDistance;
    [SerializeField] LayerMask npcLayer;

    [Header("=== DEBUG SETTINGS ===")]
    [SerializeField] bool enableDebug = true;
    Color wireSphereColor = Color.green;

    #region Sphere Raycast
    RaycastHit hit;
    #endregion

    #region Private var
    TrafficLight tl;
    #endregion

    #region Required Components 
    public SplineAnimate spline => GetComponent<SplineAnimate>();
    public CarDataManager carDataManager => GetComponent<CarDataManager>();
    #endregion
    void Update()
    {
        //SphereRayCast();
        //CheckForTrafficLight();
        ResetCar();
    }

    private void SphereRayCast()
    {
        if (Physics.SphereCast(visionPoint.position, visionRadius, transform.forward, out hit, castDistance))
        {
            if (hit.collider.GetComponent<TrafficAwareness>() != null)
            {
                wireSphereColor = Color.red;
                spline.Pause();
            }

            if (hit.collider.GetComponent<TrafficLight>() != null)
            {
                tl = hit.collider.GetComponent<TrafficLight>();
                switch (tl.IsRedLight)
                {
                    case true:
                        Debug.Log("Traffic Light Detected");
                        wireSphereColor = Color.red;
                        spline.Pause();
                        break;

                    case false:
                        wireSphereColor = Color.green;
                        spline.Play();
                        break;
                }
            }
        }
        else
        {
            tl = null;
            wireSphereColor = Color.green;
            spline.Play();
        }
    }

    private void CheckForTrafficLight()
    {
        if (tl == null) return;

       
    }

    private void ResetCar()
    {
        if (spline == null) return;
        if (spline.ElapsedTime > 32)
        {
            gameObject.SetActive(false);
            NetworkSender.instance?.DisableNPCar(carDataManager.id);
        }
    }

    private void OnDrawGizmos()
    {
        if (!enableDebug) return;

        Gizmos.color = wireSphereColor;
        Gizmos.DrawWireSphere(visionPoint.position - (-visionPoint.forward) * castDistance, visionRadius);
    }
}
