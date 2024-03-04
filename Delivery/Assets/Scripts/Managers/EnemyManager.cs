using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Car Animation")]
    [SerializeField] Transform[] frontWheelHolder;
    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelRotationMultiplier;

    private Vector3 lastPosition;
    private float lastTime;

    #region RequiredComponents
    public Rigidbody rb => GetComponent<Rigidbody>();
    #endregion

    #region Pos & Rot Setters
    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void UpdateRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
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

    public void RotateCarWheels(float speed)
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Rotate(speed, 0f, 0f);
        }
        Debug.Log($"Received Speed {speed}");
    }

    #region CarAnimation Setters

    public void FronWheelsHolderRotation(Quaternion rotation)
    {
        Debug.Log($"Received rotations {rotation}");

        for (int i = 0; i < frontWheelHolder.Length; i++)
        {
            frontWheelHolder[i].transform.localRotation = rotation;
        }
    }
    #endregion
}
