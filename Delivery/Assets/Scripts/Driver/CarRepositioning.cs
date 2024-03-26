using Driver;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CarRepositioning : MonoBehaviour
{
    [SerializeField] DriverControls driverControls;
    [SerializeField] private float recordInterval = 5f;
    private Vector3 recordedPosition;

    private void OnEnable()
    {
        StartCoroutine(RecordLastPosition());
    }

    private void Update()
    {
        TriggerReposition();
    }

    private void TriggerReposition()
    {
        if (Input.GetKeyDown(driverControls.repositionKey))
        {
            transform.position = recordedPosition;
        }
    }

    private IEnumerator RecordLastPosition()
    {
        while (true)
        {
            recordedPosition = transform.position;
            yield return new WaitForSeconds(recordInterval);
        }
    }
}