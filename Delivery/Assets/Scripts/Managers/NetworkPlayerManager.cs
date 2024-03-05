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
    }

    #region Subscribe Unsubscribe Events
    private void OnEnable()
    {
        Client.instance.onMove += SetEnemyProperties;
        Client.instance.onEnemySpawn += SpawnEnemyPlayer;
    }

    private void OnDisable()
    {
        Client.instance.onMove -= SetEnemyProperties;
        Client.instance.onEnemySpawn -= SpawnEnemyPlayer;
    }
    #endregion

    #region Spawn and Pos
    public void SpawnEnemyPlayer(SpawnEnemyPacket packet)
    {
        GameObject spawnedEnemy = Instantiate(enemyPlayerPrefab, packet.pos, Quaternion.identity);
        enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
    }

    public void SetEnemyProperties(EnemyPropertiesPacket packet)
    {
        enemyManager?.SetProperties(packet.pos, packet.rot, packet.wheelSpeed, packet.flWheelHolderRot, packet.frWheelHolderRot);
    }
    #endregion
}