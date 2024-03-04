using Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarAnimation : MonoBehaviour
{
    [SerializeField] Transform [] frontWheelHolders;
    [SerializeField] float wheelSteerSensitivity = 10f;
    [SerializeField] float rotateSpeed = 2f;

    #region private vars
    Wheel[] wheels;
    float direction;
    float currentWheelAngle;
    float wheelSpeed;
    #endregion

    #region Required Components
    public CarController carController => GetComponent<CarController>();
    public Rigidbody rb => GetComponent<Rigidbody>();
    #endregion

    #region Getters
    public float GetWheelSpeed => wheelSpeed;
    #endregion

    private void Start()
    {
        if (carController == null) return;
        wheels = carController.GetWheels;
    }

    private void Update()
    {
        if (carController == null) return;
        WheelRotations();
    }

    private void WheelRotations()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            switch (carController.GetMoveInput)
            {
                case > 0:
                    direction = 1;
                    break;
                case < 0:
                    direction = -1;
                    break;
            }

            if (wheels[i].axel == Axel.Front)
            {
                float targetSteerAngle = carController.GetSteerInput * wheelSteerSensitivity;

                for (int j = 0; j < frontWheelHolders.Length; j++)
                {
                    currentWheelAngle = Mathf.Lerp(currentWheelAngle, targetSteerAngle, Time.deltaTime * rotateSpeed);

                    frontWheelHolders[j].localRotation = Quaternion.Euler(0, currentWheelAngle, 0);
                }

                wheels[i].wheelModel.transform.Rotate(rb.velocity.magnitude * direction, 0f, 0f);
            }
            else
            {
                wheels[i].wheelModel.transform.Rotate(rb.velocity.magnitude * direction, 0f, 0f);
            }
        }
        wheelSpeed = rb.velocity.magnitude * direction;
    }
}
