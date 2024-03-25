using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Spawn other player on the map
// Update the pther player position rotation on the map

public class NetworkPlayerManager : MonoBehaviour
{
    public static NetworkPlayerManager instance;

    [SerializeField] GameObject enemyPlayerPrefab;
    EnemyManager enemyManager;

    public bool hasSpawned { get; private set; }
    public int enemySpawnPointIndex { get; private set; }
    public int enemyStartLocIndex { get; private set; }


    #region 
    ClientManager thisClient;
    #endregion

    #region Getter
    public EnemyManager GetEnemyManager => enemyManager;
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
        thisClient = ClientManager.instance;
    }

    #region Spawn and Pos
    public void SpawnEnemyPlayer(PlayerData playerData, Vector3 spawnPos, int spawnIndex)
    {
        Debug.Log("Location Received");

        if (playerData.teamNumber == thisClient.playerData.teamNumber) return;
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, spawnPos, Quaternion.identity);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
        enemySpawnPointIndex = spawnIndex;
    }

    public void SetEnemyProperties(PlayerData playerData, Vector3 pos, Quaternion rot, float wheelSpeed, Quaternion flWheelHolderRot, Quaternion frWheelHolderRot)
    {
        Debug.Log("Properties Received");

        hasSpawned = true;
        if (playerData.teamNumber == thisClient.playerData.teamNumber) return;
        enemyManager?.ReceivePropertiesFromNetwork(pos, rot, wheelSpeed, flWheelHolderRot, frWheelHolderRot);
    }
    #endregion

    #region UpdatedVersion

    public void SpawnEnemyPlayer(Vector3 spawnPos)
    {
        Debug.Log("Location Received");
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, spawnPos, Quaternion.identity);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
    }

    public void SpawnEnemyPlayer(Vector3 spawnPos, int startLocIndex, int pointIndex)
    {
        Debug.Log("Location Received");
        
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, spawnPos, Quaternion.identity);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
        enemyStartLocIndex = startLocIndex;
        enemySpawnPointIndex = pointIndex;
    }
    public void SetEnemyProperties(Vector3 pos, Quaternion rot, float wheelSpeed, Quaternion flWheelHolderRot, Quaternion frWheelHolderRot)
    {
        hasSpawned = true;
        enemyManager?.ReceivePropertiesFromNetwork(pos, rot, wheelSpeed, flWheelHolderRot, frWheelHolderRot);
    }
    #endregion
}