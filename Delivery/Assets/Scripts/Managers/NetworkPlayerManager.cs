using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawn other player on the map
// Update the pther player position rotation on the map

public class NetworkPlayerManager : MonoBehaviour
{
    public static NetworkPlayerManager instance;

    [SerializeField] GameObject enemyPlayerPrefab;
    EnemyManager enemyManager;

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


    public void SpawnEnemyPlayer(Vector3 spawnPoint, Quaternion rotation)
    {
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, spawnPoint, rotation);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
    }

    public void UpdateEnemyTransform(Vector3 position, Quaternion rotation)
    {
        enemyManager?.UpdatePosition(position);
        enemyManager?.UpdateRotation(rotation);
    }
}