using Driver;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkPlayerManager))]
public class DriverSpawnerManager : MonoBehaviour
{
    public static DriverSpawnerManager instance;

    [SerializeField] GameObject localDriver;

    [Header("=== SPAWN POINTS ===")]
    [SerializeField, NonReorderable] SpawnPoints[] spawners;
    GameObject spawnedDriver;
    int startLocationIndex;
    int pointIndex;

    [Header("=== DEBUGGING ===")]
    [SerializeField] bool showDebug = true;
    [SerializeField] float sphereRadius = .5f;

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


        if (!networkEnemyManager.hasSpawned)
        {
            startLocationIndex = Random.Range(0, spawners.Length);
            pointIndex = Random.Range(0, spawners[startLocationIndex].points.Length);
            InstantiateAndSendToNetwork(startLocationIndex);
        }
        //if enemy exists then make sure to get its index position so that it wont be thesame as you
        else
        {
            pointIndex = Random.Range(0, spawners[networkEnemyManager.enemyStartLocIndex].points.Length);
            while (pointIndex == networkEnemyManager.enemySpawnPointIndex)
            {
                pointIndex = Random.Range(0, spawners[networkEnemyManager.enemyStartLocIndex].points.Length);
            }
            InstantiateAndSendToNetwork(networkEnemyManager.enemyStartLocIndex);
        }
    }

    private void InstantiateAndSendToNetwork(int startLocationIndex)
    {
        GameObject driver = Instantiate(localDriver, spawners[startLocationIndex].points[pointIndex].position, Quaternion.Euler(spawners[startLocationIndex].points[pointIndex].forward));

        ApplyCarSkin(driver);

        driver.transform.forward = spawners[startLocationIndex].points[pointIndex].forward;
        spawnedDriver = driver;

        try
        {
            SendPackets.SpawnCar(driver.transform.position, startLocationIndex, pointIndex, ClientManager.instance?.playerData.name);
        }
        catch (System.Exception)
        {
        }
        //NetworkSender.instance?.SendSpawnEnemyPacket(transform.position, randomIndex);
    }

    private static void ApplyCarSkin(GameObject driver)
    {
        //Applying car skin
        driver.GetComponentInChildren<MeshRenderer>().material.SetTexture("_BaseMap", DriverItemShopHandler.instance?.GetPlayerChosenCarColor());
        driver.transform.Find("SM_Veh_Car_Van_Door_l").GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", DriverItemShopHandler.instance?.GetPlayerChosenCarColor());
        driver.transform.Find("SM_Veh_Car_Van_Door_r").GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", DriverItemShopHandler.instance?.GetPlayerChosenCarColor());
    }

    public void DisableSpawnedDriver()
    {
        if (spawnedDriver == null) return;
        spawnedDriver.GetComponent<CarController>().enabled = false;
        spawnedDriver.GetComponent<SendCarPropertiesToNetwork>().enabled = false;
        spawnedDriver.GetComponent<CarMalfunction>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (!showDebug) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < spawners.Length; i++)
        {
            for (int j = 0; j < spawners[i].points.Length; j++)
            {
                Gizmos.DrawSphere(spawners[i].points[j].position, sphereRadius);
            }
        }
    }
}

[System.Serializable]
public class SpawnPoints
{
    public Transform[] points;
}
