using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOutsideVehicles : MonoBehaviour
{
    [SerializeField, MinMaxSlider(5, 10)] Vector2 spawnTime;
    [SerializeField] Transform[] vehicles;
    [SerializeField] Transform startPoint, endPoint;
    [SerializeField] float vehicleSpeed = 10f;

    int randomIndex;
    Transform chosenVehicleTransform;
    private void Start()
    {
        DisableVehicles();
        StartCoroutine(DrivingVehicleLoop());
    }

    private void Update()
    {
        MoveActiveVehicleToEndPoint();
    }

    private void MoveActiveVehicleToEndPoint()
    {
        if (chosenVehicleTransform == null) return;


        if (Vector3.Distance(chosenVehicleTransform.position, endPoint.position) > .5f)
        {
            chosenVehicleTransform.position = Vector3.MoveTowards(chosenVehicleTransform.position, endPoint.position, Time.deltaTime * vehicleSpeed);
        }
        else
        {
            chosenVehicleTransform.gameObject.SetActive(false);
            chosenVehicleTransform = null;
        }
    }

    private void DisableVehicles()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].gameObject.SetActive(false);
        }
    }

    IEnumerator DrivingVehicleLoop()
    {
        while (true)
        {
            float randomTime = Random.Range(spawnTime.x, spawnTime.y);
            yield return new WaitForSeconds(randomTime);
            if (chosenVehicleTransform == null)
            {
                ChooseARandomVehicle();
            }
        }
    }

    void ChooseARandomVehicle()
    {
        do
        {
            randomIndex = Random.Range(0, vehicles.Length);
        } while (vehicles[randomIndex].gameObject.activeSelf);

        vehicles[randomIndex].gameObject.SetActive(true);
        chosenVehicleTransform = vehicles[randomIndex];
        chosenVehicleTransform.transform.position = startPoint.position;
    }
}
