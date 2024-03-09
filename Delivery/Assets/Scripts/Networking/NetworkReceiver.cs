using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkReceiver : MonoBehaviour
{
    public static NetworkReceiver instance { get; private set; }
    Client client;
    void Awake()
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

    private void OnEnable()
    {
        client.onLobbyJoined += UpdatePlayerListAndSendNameToNetwork;
        client.onPlayerTeamAndRole += UpdateLobbyUIManager;
        client.onChangeTeam += UpdateChangedRolesFromNetwork;
        client.onGameStart += ReceivePacketIfGameHasStarted;

        client.onDeliveryAddress += SetDeliveryLocation;
        client.onEnemySpawn += SpawnEnemyPlayer;
        client.onMove += SetEnemyProperties;

        client.onDriverCollision += TriggerRandomMapRotation;

        client.onDriverArrived += DeclareWinner;
    }

    private void OnDisable()
    {
        client.onLobbyJoined -= UpdatePlayerListAndSendNameToNetwork;
        client.onPlayerTeamAndRole -= UpdateLobbyUIManager;
        client.onChangeTeam -= UpdateChangedRolesFromNetwork;
        client.onGameStart -= ReceivePacketIfGameHasStarted;

        client.onDeliveryAddress -= SetDeliveryLocation;
        client.onEnemySpawn -= SpawnEnemyPlayer;
        client.onMove -= SetEnemyProperties;

        client.onDriverCollision -= TriggerRandomMapRotation;

        client.onDriverArrived -= DeclareWinner;
    }

    #region LobbyReceivers
    void UpdatePlayerListAndSendNameToNetwork(JoinServerPacket packet)
    {
        PlayerLobbyManager.instance?.UpdatePlayerListAndSendNameToNetwork(packet.playerData);
    }

    void UpdateLobbyUIManager(TeamAndRolePacket packet)
    {
        LobbyUIManager.instance?.UpdateLobbyUIManager(packet.playerData);
    }

    void UpdateChangedRolesFromNetwork(ChangeTeamPacket packet)
    {
        LobbyUIManager.instance?.UpdateChangedRolesFromNetwork(packet.playerData);
    }

    void ReceivePacketIfGameHasStarted(GameStartPacket packet)
    {
        LobbyUIManager.instance?.ReceivePacketIfGameHasStarted(packet.gameHasStarted);
    }
    #endregion

    #region Driver Receivers
    void SetDeliveryLocation(DeliveryLocationPacket packet)
    {
        LocationHandler.instance?.SetDeliveryLocation(packet.playerData, packet.buildingName);
    }

    void SpawnEnemyPlayer(SpawnEnemyPacket packet)
    {
        NetworkPlayerManager.instance?.SpawnEnemyPlayer(packet.playerData, packet.pos, packet.spawnIndex);
    }

    void SetEnemyProperties(EnemyPropertiesPacket packet)
    {
        NetworkPlayerManager.instance?.SetEnemyProperties(packet.playerData, packet.pos, packet.rot, packet.wheelSpeed, packet.flWheelHolderRot, packet.frWheelHolderRot);
    }
    #endregion

    #region Navigator Receivers
    public void TriggerRandomMapRotation(DriverCollidedPacket packet)
    {
        DriverCollisionHandler.instance?.TriggerRandomMapRotation(packet.hasCollided, packet.playerData);
    }
        #endregion

    #region Win Receivers
        public void DeclareWinner(DriverArrivedPacket packet)
    {
        WinManager.instance?.DeclareWinner(packet.hasArrived, packet.playerData);
    }
    #endregion

}
