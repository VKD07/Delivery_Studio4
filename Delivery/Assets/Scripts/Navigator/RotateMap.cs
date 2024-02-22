using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapActivator))]
public class RotateMap : MonoBehaviour
{
    [SerializeField] NavigatorControls navControls;
    [Range(50, 300)]
    [SerializeField] float sensitivity = .5f;
    Vector3 rotation;
    Quaternion currentMapRot;
    

    #region Required Components
    public MapActivator mapManager => GetComponent<MapActivator>();
    #endregion

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (Input.GetKey(navControls.xRotationKey))
        {
            rotation.y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            mapManager.GetActiveMap.transform.Rotate(new Vector3(rotation.y, 0, 0), Space.World);
            currentMapRot = mapManager.GetActiveMap.transform.localRotation;
        }
        else if (Input.GetKey(navControls.yRotationKey))
        {
            rotation.x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mapManager.GetActiveMap.transform.localRotation *= Quaternion.Euler(0, -rotation.x, 0);
            currentMapRot = mapManager.GetActiveMap.transform.localRotation;
        }
    }

    #region Setters
    public void ApplyCurrentRotationToAllMaps()
    {
        mapManager.GetActiveMap.transform.rotation = currentMapRot;
    }
    #endregion
}
