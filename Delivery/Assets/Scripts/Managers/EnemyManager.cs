using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Car Animation")]
    [SerializeField] Transform flWheelHolder, frWheelHolder;
    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelRotationMultiplier;

    private Vector3 lastPosition;
    private float lastTime;

    #region RequiredComponents
    public Rigidbody rb => GetComponent<Rigidbody>();
    #endregion

    #region Setters
    public void SetProperties(Vector3 position, Quaternion rot, float wheelSpeed, Quaternion flWheelRot, Quaternion frWheelRot)
    {
        transform.position = position;
        transform.rotation = rot;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Rotate(wheelSpeed, 0f, 0f);
        }

        flWheelHolder.localRotation = flWheelRot;
        frWheelHolder.localRotation = frWheelRot;
    }
    #endregion

    void Start()
    {
        // Initialize lastPosition and lastTime
        lastPosition = transform.position;
        lastTime = Time.time;
    }

    private void Update()
    {
        //// Calculate elapsed time since last check
        //float deltaTime = Time.time - lastTime;

        //// Calculate distance traveled since last check
        //float distance = Vector3.Distance(transform.position, lastPosition);

        //// Calculate speed: distance divided by time
        //float speed = distance / deltaTime;

        //// Output the speed
        //Debug.Log("Speed: " + speed + " units per second");

        //// Update lastPosition and lastTime for next check
        //lastPosition = transform.position;
        //lastTime = Time.time;

        //RotateCarWheels(speed);
    }
}
