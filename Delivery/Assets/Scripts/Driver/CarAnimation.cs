using Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarAnimation : MonoBehaviour
{
    [SerializeField] float wheelYRotationMultiplayer = 30f;
    [SerializeField] float wheelXRotationMultiplayer = 100f;
    #region private vars
    float direction;
    Wheel[] wheels;
    #endregion

    #region Required Components
    public CarController carController => GetComponent<CarController>();
    public Rigidbody rb => GetComponent<Rigidbody>();
    #endregion

    private void Start()
    {
        if (carController == null) return;
        wheels = carController.GetWheels;
    }

    private void Update()
    {
        if (carController == null) return;
        RotateWheelsForwardBackwards();
        //SteerFrontWheels();
    }

    private void RotateWheelsForwardBackwards()
    {
        Debug.Log(rb.velocity.magnitude * direction * wheelXRotationMultiplayer);
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
                // Rotate front wheels based on steering input and forward movement
                float steerAngle = carController.GetSteerInput * wheelYRotationMultiplayer;
              
                wheels[i].wheelModel.transform.Rotate(rb.velocity.magnitude * direction, 0f,0f, Space.Self);
                //Quaternion frontRotation = Quaternion.Euler(0f, steerAngle, 0f);
                //wheels[i].wheelModel.transform.localRotation = frontRotation;
            }
            else
            {
                // Rotate rear wheels based only on forward movement
                //Quaternion rearRotation = Quaternion.Euler(rb.velocity.magnitude * direction * wheelXRotationMultiplayer, 0f, 0f);
                //wheels[i].wheelModel.transform.localRotation = rearRotation;
                wheels[i].wheelModel.transform.Rotate(rb.velocity.magnitude * direction, 0f, 0f);
            }
        }
    }

    //private void SteerFrontWheels()
    //{

    //    for (int i = 0; i < wheels.Length; i++)
    //    {
    //        if (wheels[i].axel == Axel.Front)
    //        {
    //            wheels[i].wheelModel.transform.localRotation =
    //                Quaternion.Euler(wheels[i].wheelModel.transform.localEulerAngles.x,
    //                steeringWheel.transform.localEulerAngles.y * .5f,
    //                0f);
    //        }
    //    }

    //}
}
