using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using GD.MinMaxSlider;
using TMPro;

public class CarNPCSpawner : MonoBehaviour
{
    public static CarNPCSpawner instance;

    [Header("=== SPLINE ===")]
    [SerializeField, Tooltip("This spline will be applied to all spawned cars")]
    SplineContainer[] splinePaths;

    [Header("=== SPAWNING SETTINGS ===")]
    [MinMaxSlider(0, 20)]
    [SerializeField] Vector2 spawnTime;

    [SerializeField] int initNumOfCarsToSpawn = 3;

    [Header("=== SPAWNED CARS ===")]
    [SerializeField] GameObject[] carsToSpawn;

    [Header("=== DEBUG SETTINGS ===")]
    [SerializeField] Color spherColor = Color.red;
    [SerializeField] float sphereRad = .5f;

    #region private Vars
    bool otherPlayerAlreadySpawned;
    Dictionary<int, GameObject> poolOfCars = new Dictionary<int, GameObject>();
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        StartCoroutine(SpawnPoolOfCars());
    }
    private void Start()
    {
        StartCoroutine(SpawnCarsRandomly());
    }

    IEnumerator SpawnPoolOfCars()
    {
        //This is to avoid both drivers spawning npc cars together, It should be randomize so that,
        //if one player instantiates first then other driver will receive the pool of npc cars to spawn
        int randomTime = Random.Range(0, 5);
        yield return new WaitForSeconds(randomTime);

        if (!otherPlayerAlreadySpawned)
        {
            for (int i = 0; i < initNumOfCarsToSpawn; i++)
            {
                int randomCarIndex = Random.Range(0, carsToSpawn.Length);
                GameObject spawnedCar = Instantiate(carsToSpawn[randomCarIndex], transform.position, Quaternion.identity);

                spawnedCar.transform.forward = transform.forward;
                SplineAnimate splineAnimate = spawnedCar.GetComponent<SplineAnimate>();
                NPCCarManager dataManager = spawnedCar.GetComponent<NPCCarManager>();

                if (splineAnimate != null)
                {
                    int randomPath = Random.Range(0, splinePaths.Length);
                    splineAnimate.Container = splinePaths[randomPath];
                }

                if (dataManager != null)
                {
                    dataManager.id = i;
                }

                spawnedCar.SetActive(false);
                poolOfCars.Add(i, spawnedCar);
                SendPackets.SendNPCCars(randomCarIndex, i, true);
                //NetworkSender.instance?.InstantiateNPCCar(randomCarIndex, i, true);
            }
        }
    }


    IEnumerator SpawnCarsRandomly()
    {
        yield return new WaitForSeconds(3);
        if (!otherPlayerAlreadySpawned) //make sure to not spawn when the other player is already spawning
        {
            while (true)
            {
                float randomSpawnTime = Random.Range(spawnTime.x, spawnTime.y);
                yield return new WaitForSeconds(randomSpawnTime);
                ChooseACarToEnable();
            }
        }
    }

    void ChooseACarToEnable()
    {
        int chosenCarIndex = Random.Range(0, poolOfCars.Count);
        while (poolOfCars[chosenCarIndex].activeSelf)
        {
            chosenCarIndex = Random.Range(0, poolOfCars.Count);
        }
        SplineAnimate splineAnimate = poolOfCars[chosenCarIndex].GetComponent<SplineAnimate>();

        splineAnimate.Restart(true);
        splineAnimate.Play();
        poolOfCars[chosenCarIndex].SetActive(true);
    }

    #region Network Receivers
    public void InstantiateNpcCar(int carIndex, int id, bool hasSpawned)
    {
        if (hasSpawned)
        {
            otherPlayerAlreadySpawned = hasSpawned;
        }

        GameObject spawnedCar = Instantiate(carsToSpawn[carIndex], transform.position, Quaternion.identity);

        spawnedCar.transform.forward = transform.forward;

        SplineAnimate splineAnimate = spawnedCar.GetComponent<SplineAnimate>();

        if (splineAnimate != null)
        {
            splineAnimate.Container = splinePaths[0];
            splineAnimate.enabled = false;
        }

        spawnedCar.SetActive(false);
        poolOfCars.Add(id, spawnedCar);
    }

    public void UpdateCarPropertiesOnTheList(int id, Vector3 pos, Quaternion rot)
    {
        if (!poolOfCars[id].activeSelf)
        {
            poolOfCars[id].SetActive(true);
        }
        poolOfCars[id].GetComponent<NPCCarManager>().UpdateTransform(pos, rot);
    }

    public void DisableNPCCar(int id, bool val)
    {
        poolOfCars[id].SetActive(false);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = spherColor;
        Gizmos.DrawSphere(transform.position, sphereRad);
    }
}