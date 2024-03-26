using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform playerCamera;
    private void Update()
    {
        if (playerCamera == null)
        {
            try
            {
                playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
            }
            catch (System.Exception)
            {
            }
        }

        if (playerCamera == null) return;
        transform.LookAt(playerCamera.position);
    }
}
