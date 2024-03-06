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

    #region 
    Client client;
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
        client = Client.instance;
    }

    #region Subscribe Unsubscribe Events
    private void OnEnable()
    {
        if (client == null) return;
        client.onMove += SetEnemyProperties;
        client.onEnemySpawn += SpawnEnemyPlayer;
    }

    private void OnDisable()
    {
        if (client == null) return;
        client.onMove -= SetEnemyProperties;
        client.onEnemySpawn -= SpawnEnemyPlayer;
    }
    #endregion

    #region Spawn and Pos
    public void SpawnEnemyPlayer(SpawnEnemyPacket spawnEnemyPacket)
    {
        Debug.Log("Location Received");
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, spawnEnemyPacket.pos, Quaternion.identity);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
    }

    public void SetEnemyProperties(EnemyPropertiesPacket packet)
    {
        enemyManager?.SetProperties(packet.pos, packet.rot, packet.wheelSpeed, packet.flWheelHolderRot, packet.frWheelHolderRot);
    }

    //public void SetEnemyProperties(Vector3 pos, Quaternion rot, float wheelSpeed, Quaternion flRot, Quaternion frRot)
    //{
    //    enemyManager?.SetProperties(pos, rot, wheelSpeed, flRot, frRot);
    //}

    //public void SpawnEnemyPlayer(Vector3 pos)
    //{
    //    Debug.Log("Location Received");
    //    //SpawnEnemyPacket spawnEnemyPacket = (SpawnEnemyPacket)packet;
    //    GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, pos, Quaternion.identity);
    //    enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
    //}
    #endregion
}