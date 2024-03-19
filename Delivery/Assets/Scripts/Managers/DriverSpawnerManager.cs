using Driver;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(NetworkPlayerManager))]
public class DriverSpawnerManager : MonoBehaviour
{
    public static DriverSpawnerManager instance;

    [SerializeField] GameObject localDriver;
    [SerializeField] Transform[] spawnPoints;
    GameObject spawnedDriver;
    int randomIndex;
    public NetworkPlayerManager networkEnemyManager => GetComponent<NetworkPlayerManager>();

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
    }

    private void Start()
    {
        StartCoroutine(RandomizeSpawnPos());
    }

    IEnumerator RandomizeSpawnPos()
    {
        //One of them will spawn first, and if someone spawn first, they need to tell the other driver what spawn point he took so that 
        //Other player wont be in thesame spawn point
        int randomTime = Random.Range(0, 4);
        yield return new WaitForSeconds(randomTime);
        //if enemy exists then make sure to get its index position so that it wont be thesame as you
        randomIndex = Random.Range(0, spawnPoints.Length);

        if (networkEnemyManager.hasSpawned)
        {
            while (randomIndex == networkEnemyManager.enemySpawnIndex)
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            }
        }
        InstantiateAndSendToNetwork();
    }

    private void InstantiateAndSendToNetwork()
    {
        GameObject driver = Instantiate(localDriver, spawnPoints[randomIndex].position, Quaternion.Euler(spawnPoints[randomIndex].forward));
        driver.transform.forward = spawnPoints[randomIndex].forward;
        spawnedDriver = driver;
        SendPackets.SpawnCar(transform.position, randomIndex);
        //NetworkSender.instance?.SendSpawnEnemyPacket(transform.position, randomIndex);
    }

    public void DisableSpawnedDriver()
    {
        if (spawnedDriver == null) return;
        spawnedDriver.GetComponent<CarController>().enabled = false;
        spawnedDriver.GetComponent<SendCarPropertiesToNetwork>().enabled = false;
        spawnedDriver.GetComponent<CarMalfunction>().enabled = false;
    }
}
