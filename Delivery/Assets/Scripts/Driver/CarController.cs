using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Driver
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

    public class CarController : MonoBehaviour
    {
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
        [SerializeField] float steeringSpeed = 10f;
        [SerializeField] float maxSteeringAngle = 10f;
        float currentSteerAngle;

        Rigidbody rb => GetComponent<Rigidbody>();

        #region Getters
        public Wheel[] GetWheels => wheelList.ToArray();
        public float GetMoveInput => moveInput;
        public float GetSteerInput => steerInput;
        public float GetTurnSensitivity => turnSensitivity;
        public float GetMaxSteerAngle => maxSteeringAngle;
        #endregion

        private void Start()
        {
            rb.centerOfMass = centerOfMass;
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
            float targetSteerAngle = steerInput * steeringSensitivity * maxSteeringAngle;

            currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steeringSpeed);

            steeringWheel.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
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
