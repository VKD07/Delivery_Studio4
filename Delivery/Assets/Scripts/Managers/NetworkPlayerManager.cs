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
        Client.instance.onMove += UpdateEnemyTransform;
        Client.instance.onWheelHolderRot += UpdateEnemyFrontWheelHolder;
        //Client.instance.onWheelSpeed += UpdateWheelRotation;
    }

    private void OnDisable()
    {
        Client.instance.onMove -= UpdateEnemyTransform;
        Client.instance.onWheelHolderRot -= UpdateEnemyFrontWheelHolder;
        //Client.instance.onWheelSpeed -= UpdateWheelRotation;
    }
    #endregion

    #region Spawn and Pos
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
    #endregion

    #region Car Animation


    public void UpdateEnemyFrontWheelHolder(Quaternion frontWheelHolder)
    {
        enemyManager?.FronWheelsHolderRotation(frontWheelHolder);
    }

    public void UpdateWheelRotation(float wheelSpeed)
    {
        enemyManager?.RotateCarWheels(wheelSpeed);
    }
    #endregion
}