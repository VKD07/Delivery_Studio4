using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShopUIRot : MonoBehaviour
{
    [SerializeField] Vector3[] minMaxRot;
    [SerializeField] float rotSensitivity = 1f;

    float mouseX;
    float newRotationY;
    float clampedRotationY;

    void LateUpdate()
    {
        mouseX = Input.GetAxis("Mouse X") * rotSensitivity * Time.deltaTime;
        newRotationY = transform.rotation.eulerAngles.y - mouseX;
        clampedRotationY = Mathf.Clamp(newRotationY, minMaxRot[0].y, minMaxRot[1].y);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, clampedRotationY, transform.rotation.eulerAngles.z);
    }
}
