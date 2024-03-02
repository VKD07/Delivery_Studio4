using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using GD.MinMaxSlider;

public class CarNPCSpawner : MonoBehaviour
{
    [Header("=== SPLINE ===")]
    [SerializeField, Tooltip("This spline will be applied to all spawned cars")]
    SplineContainer splinePath;

    [Header("=== SPAWNING SETTINGS ===")]
    [MinMaxSlider(0, 20)]
    [SerializeField] Vector2 spawnTime;
    [SerializeField] float carSpeed = 10;

    [SerializeField] int initNumOfCarsToSpawn = 3;

    [Header("=== SPAWNED CARS ===")]
    [SerializeField] GameObject[] carsToSpawn;
    [Space]
    [SerializeField] List<GameObject> poolOfCars;
    int activeCars;


    [Header("=== DEBUG SETTINGS ===")]
    [SerializeField] Color spherColor = Color.red;
    [SerializeField] float sphereRad = .5f;

    private void Awake()
    {
        SpawnPoolOfCars();
    }
    private void Start()
    {
        StartCoroutine(SpawnCarsRandomly());
    }

    void SpawnPoolOfCars()
    {
        for (int i = 0; i < initNumOfCarsToSpawn; i++)
        {
            int randomCarIndex = Random.Range(0, carsToSpawn.Length);
            GameObject spawnedCar = Instantiate(carsToSpawn[randomCarIndex], transform.position, Quaternion.identity);

            spawnedCar.transform.forward = transform.forward;

            SplineAnimate splineAnimate = spawnedCar.GetComponent<SplineAnimate>();
            if (splineAnimate == null) return;
            splineAnimate.MaxSpeed = carSpeed;
            splineAnimate.Container = splinePath;

            spawnedCar.SetActive(false);
            poolOfCars.Add(spawnedCar);
        }
    }

    IEnumerator SpawnCarsRandomly()
    {
        while (true)
        {
            float randomSpawnTime = Random.Range(spawnTime.x, spawnTime.y);
            yield return new WaitForSeconds(randomSpawnTime);
            ChooseACarToEnable();
        }
    }

    void ChooseACarToEnable()
    {
        int chosenCarIndex = Random.Range(0, poolOfCars.Count);
        while (poolOfCars[chosenCarIndex].activeSelf)
        {
            chosenCarIndex = Random.Range(0, poolOfCars.Count);
        }
        poolOfCars[chosenCarIndex].GetComponent<SplineAnimate>().Restart(true);
        poolOfCars[chosenCarIndex].GetComponent<SplineAnimate>().Play();
        poolOfCars[chosenCarIndex].SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = spherColor;
        Gizmos.DrawSphere(transform.position, sphereRad);
    }
}
