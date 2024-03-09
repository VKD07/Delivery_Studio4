using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkPlayerManager))]
public class DriverSpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject localDriver;
    [SerializeField] Transform[] spawnPoints;

    public NetworkPlayerManager networkEnemyManager => GetComponent<NetworkPlayerManager>();
    int randomIndex;
    private void Start()
    {
        RandomizeSpawnPos();
    }

    void RandomizeSpawnPos()
    {
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

        NetworkSender.instance?.SendSpawnEnemyPacket(transform.position, randomIndex);
    }
}
