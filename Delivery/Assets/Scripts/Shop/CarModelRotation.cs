using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarModelRotation : MonoBehaviour, MouseSelection
{
    [SerializeField] Transform carModel;
    [SerializeField] float rotationSensitivity = 5f;
    float horizontal;
    public void TriggerClickEffect()
    {
    }

    public void TriggerHoverEffect()
    {
    }

    public void TriggerMouseHoldEffect()
    {
        RotateCarHorizontally();
    }

    private void RotateCarHorizontally()
    {
        horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSensitivity;
        Vector3 newRot = new Vector3(0f, -horizontal, 0f);
        transform.Rotate(newRot);
    }
}
