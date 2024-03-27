using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainMenuCar : MonoBehaviour
{
    [SerializeField] float carSpeed;
    Rigidbody rb => GetComponent<Rigidbody>();
   

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        rb.velocity = transform.forward * Time.deltaTime * carSpeed;
    }
}
