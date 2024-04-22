using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TrafficAwareness : MonoBehaviour
{
    [Header("=== AWARENESS SETTINGS ===")]
    [SerializeField] Transform visionPoint;
    [SerializeField] float visionRadius = 5f;
    [SerializeField] LayerMask layerToDetect;

    [Header("=== Audio ===")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip horn;

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
    public NPCCarManager carDataManager => GetComponent<NPCCarManager>();
    #endregion
    void Update()
    {
        SphereRayCast();
    }

    private void SphereRayCast()
    {
        if (Physics.CheckSphere(visionPoint.position, visionRadius, layerToDetect))
        {
            audioSource.PlayOneShot(horn, 1);
        }
    }
    private void OnDrawGizmos()
    {
        if (!enableDebug) return;

        Gizmos.color = wireSphereColor;
        Gizmos.DrawWireSphere(visionPoint.position, visionRadius);
    }
}
