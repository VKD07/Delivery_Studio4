using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Driver
{
    public class CarController : MonoBehaviour
    {
        #region Structs and Enums
        public enum Axel
        {
            Front,
            Rear
        }

        [Serializable]
        public struct Wheel
        {
            public GameObject wheelModel;
            public WheelCollider wheelCollider;
            public Axel axel;
        }
        #endregion

        [Header("=== CAR CONTROLS ===")]
        [SerializeField] DriverControls driverControls;

        [Header("=== CAR SETTING ===")]
        [SerializeField] float maxAccel = 30f;
        [SerializeField] float brakeAccel = 50f;
        [SerializeField] float turnSensitivity = 1f;
        [SerializeField] float maxSteerAngle = 30f;
        [SerializeField] Vector3 centerOfMass;

        [Header("=== WHEELS ===")]
        [SerializeField] List<Wheel> wheelList;
        float moveInput;
        float steerInput;

        [Header("=== STEERING WHEEL ===")]
        [SerializeField] Transform steeringWheel;
        [SerializeField] float steeringSensitivity = 80f;
        [SerializeField] float steeringRestoreTime = 10f;
        Quaternion initRot;

        Rigidbody rb => GetComponent<Rigidbody>();

        private void Start()
        {
            rb.centerOfMass = centerOfMass;
            initRot = steeringWheel.localRotation;
        }
        private void Update()
        {
            GetInputs();
        }

        private void LateUpdate()
        {
            Move();
            Steer();
            SteeringWheel();
            Break();
        }

        void GetInputs()
        {
            moveInput = Input.GetAxis(driverControls.vertical.ToString());
            steerInput = Input.GetAxis(driverControls.horizontal.ToString());
        }

        void Move()
        {
            for (int i = 0; i < wheelList.Count; i++)
            {
                wheelList[i].wheelCollider.motorTorque = moveInput * 600 * maxAccel * Time.deltaTime;
            }
        }

        void Steer()
        {
            for (int i = 0; i < wheelList.Count; i++)
            {
                if (wheelList[i].axel == Axel.Front)
                {
                    var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                    wheelList[i].wheelCollider.steerAngle = Mathf.Lerp(wheelList[i].wheelCollider.steerAngle, steerAngle, .6f);
                }
            }
        }

        void SteeringWheel()
        {
            if (steerInput > 0 || steerInput < 0)
            {
                steeringWheel.Rotate(new Vector3(0, steerInput * steeringSensitivity * Time.deltaTime, 0));
            }
            else
            {
                steeringWheel.localRotation = Quaternion.Lerp(steeringWheel.localRotation, initRot, 5 * Time.deltaTime);
            }
        }

        void Break()
        {
            if (Input.GetKey(driverControls.breakKey))
            {
                for (int i = 0; i < wheelList.Count; i++)
                {
                    wheelList[i].wheelCollider.brakeTorque = 300 * brakeAccel * Time.deltaTime;
                }
            }
            else
            {
                for (int i = 0; i < wheelList.Count; i++)
                {
                    wheelList[i].wheelCollider.brakeTorque = 0;
                }
            }
        }
    }
}
