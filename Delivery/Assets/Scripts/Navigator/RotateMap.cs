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

    #region Private Vars
    int x;
    int y;
    #endregion

    #region Required Components
    public MapActivator mapManager => GetComponent<MapActivator>();
    #endregion

    void Update()
    {
        Rotate();

        if (Input.GetKeyDown(KeyCode.O))
        {
            TriggerRandomRotation();
        }
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

    public void TriggerRandomRotation()
    {
        int xOry = Random.Range(0, 2);
        switch (xOry)
        {
            case 0:
                x = Random.Range(0, 180);
                break;
            case > 0:
                y = Random.Range(0, 180);
                break;
        }
        //mapManager.GetActiveMap.transform.Rotate(new Vector3(x,y,0));
        mapManager.GetActiveMap.transform.localRotation *= Quaternion.Euler(x, y, 0);
    }
    #endregion
}
