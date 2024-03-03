using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Vector3 position;
    Vector3 rotation;

    #region Getters
    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void UpdateRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    #endregion
}
