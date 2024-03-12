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
    public int enemySpawnIndex { get; private set; }

    #region 
    Client thisClient;
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
        thisClient = Client.instance;
    }

    #region Spawn and Pos
    public void SpawnEnemyPlayer(PlayerData playerData, Vector3 spawnPos, int spawnIndex)
    {
        Debug.Log("Location Received");

        if (playerData.teamNumber == thisClient.playerData.teamNumber) return;
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, spawnPos, Quaternion.identity);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
        enemySpawnIndex = spawnIndex;
    }

    public void SetEnemyProperties(PlayerData playerData, Vector3 pos, Quaternion rot, float wheelSpeed, Quaternion flWheelHolderRot, Quaternion frWheelHolderRot)
    {
        hasSpawned = true;
        if (playerData.teamNumber == thisClient.playerData.teamNumber) return;
        enemyManager?.ReceivePropertiesFromNetwork(pos, rot, wheelSpeed, flWheelHolderRot, frWheelHolderRot);
    }
    #endregion
}