using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseSelectionManager : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    MouseSelection selectedObj;
    private void Update()
    {
        OnMouseHover();
    }

    private void OnMouseHover()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.TryGetComponent<MouseSelection>(out selectedObj))
            {
                selectedObj.TriggerHoverEffect();
            }

            if (selectedObj != null && Input.GetMouseButtonDown(0))
            {
                selectedObj.TriggerClickEffect();
            }

            if(selectedObj != null && Input.GetMouseButton(0))
            {
                selectedObj.TriggerMouseHoldEffect();
            }
        }
    }
}
