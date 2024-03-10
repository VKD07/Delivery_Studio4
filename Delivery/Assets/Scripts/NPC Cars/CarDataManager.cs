using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CarDataManager : MonoBehaviour
{
    public int id;
    public SplineAnimate spline => GetComponent<SplineAnimate>();

    private void Update()
    {
        SendCurrentTransformToNetwork();
        ResetRoute();
    }

    private void SendCurrentTransformToNetwork()
    {
        NetworkSender.instance?.SendNPCCarTransform(id, transform.position, transform.rotation);
    }

    private void ResetRoute()
    {
        if (gameObject.activeSelf)
        {
            if (spline != null && spline.enabled)
            {
                if (spline.ElapsedTime >= 32)
                {
                    gameObject.SetActive(false);
                    NetworkSender.instance?.DisableNPCar(id);

                }
            }
        }
    }

    public void UpdateTransform(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
