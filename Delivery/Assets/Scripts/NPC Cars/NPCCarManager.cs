using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Splines;

public class NPCCarManager : MonoBehaviour
{
    public int id;

    [Header("=== WHEEL ROTATION")]
    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelSpeed = 10f;
    public SplineAnimate spline => GetComponent<SplineAnimate>();
    Transform trans;

    private void Awake()
    {
        trans = transform;
    }

    private void Update()
    {
        SendCurrentTransformToNetwork();
        ResetRoute();
        RotateWheels();
    }

    private void RotateWheels()
    {
        if (!gameObject.activeSelf) return;
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Rotate(new Vector3(1, 0, 0), wheelSpeed * Time.deltaTime);
        }
    }

    private void SendCurrentTransformToNetwork()
    {
        //NetworkSender.instance?.SendNPCCarTransform(id, trans.position, trans.rotation);
        SendPackets.SendNPCCarTransform(id, trans.position, trans.rotation);
    }

    private void ResetRoute()
    {
        if (gameObject.activeSelf)
        {
            if (spline != null && spline.enabled)
            {
                if (spline.ElapsedTime >= 32)
                {
                    //NetworkSender.instance?.DisableNPCar(id);
                    SendPackets.DisableNPCCar(id);
                    SendPackets.DisableNPCCar(id);
                    spline.Restart(true);
                    gameObject.SetActive(false);
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
