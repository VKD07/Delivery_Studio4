using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("=== CAR USERNAME ===")]
    [SerializeField] TextMeshProUGUI userName;

    [Header("=== CAR ANIMATION ===")]
    [SerializeField] Transform flWheelHolder, frWheelHolder;
    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelRotationMultiplier;

    #region RequiredComponents
    public Rigidbody rb => GetComponent<Rigidbody>();
    #endregion

    #region Setters
    public void ReceivePropertiesFromNetwork(Vector3 position, Quaternion rot, float wheelSpeed, Quaternion flWheelRot, Quaternion frWheelRot)
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

    public void SetEnemyUserName(string userName)
    {
        this.userName.text = userName;
    }
    #endregion
}
