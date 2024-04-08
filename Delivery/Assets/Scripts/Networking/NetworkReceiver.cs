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
        client.onTimerPacket += SetCurrentTimer;

        client.onDriverCollision += TriggerRandomMapRotation;
        client.onDirtPacket += EnableDirtScreen;

        client.onDriverArrived += DeclareWinner;

        #region NPC Car events
        client.onNPCCarSpawn += InstantiateNPCCar;
        client.onNPCTransform += EnableAndUpdateNPCCar;
        client.OnDisableNPCCar += DisableNPCCar;
        #endregion

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
        client.onTimerPacket -= SetCurrentTimer;

        client.onDriverCollision -= TriggerRandomMapRotation;
        client.onDirtPacket -= EnableDirtScreen;

        client.onDriverArrived -= DeclareWinner;

        client.onNPCCarSpawn -= InstantiateNPCCar;
        client.onNPCTransform -= EnableAndUpdateNPCCar;
        client.OnDisableNPCCar -= DisableNPCCar;
    }

    #region LobbyReceivers
    void UpdatePlayerListAndSendNameToNetwork(JoinServerPacket packet)
    {
        //PlayerLobbyManager.instance?.UpdatePlayerListAndSendNameToNetwork(packet.playerData);
    }

    void UpdateLobbyUIManager(TeamAndRolePacket packet)
    {
        //LobbyUIManager.instance?.UpdateLobbyUIManager(packet.playerData);
    }

    void UpdateChangedRolesFromNetwork(ChangeTeamPacket packet)
    {
        //LobbyUIManager.instance?.UpdateChangedRolesFromNetwork(packet.playerData);
    }

    void ReceivePacketIfGameHasStarted(GameStartPacket packet)
    {
        //LobbyUIManager.instance?.ReceivePacketIfGameHasStarted(packet.gameHasStarted);
    }
    #endregion

    #region Driver Receivers
    void SetDeliveryLocation(DeliveryLocationPacket packet)
    {
        //LocationHandler.instance?.SetDeliveryLocation(packet.playerData, packet.buildingName);
    }

    void SpawnEnemyPlayer(SpawnEnemyPacket packet)
    {
        //NetworkPlayerManager.instance?.SpawnEnemyPlayer(packet.playerData, packet.pos, packet.spawnIndex);
    }

    void SetEnemyProperties(EnemyPropertiesPacket packet)
    {
        //NetworkPlayerManager.instance?.SetEnemyProperties(packet.playerData, packet.pos, packet.rot, packet.wheelSpeed, packet.flWheelHolderRot, packet.frWheelHolderRot);
    }

    public void SetCurrentTimer(TimerPacket packet)
    {
        //TimerManager.instance?.UpdateTimer(packet.currentTime, packet.playerData);
    }
    #endregion

    #region Navigator Receivers
    public void TriggerRandomMapRotation(DriverCollidedPacket packet)
    {
        //DriverCollisionHandler.instance?.TriggerRandomMapRotation(packet.hasCollided, packet.playerData);
    }

    public void EnableDirtScreen(DirtScreenPacket packet)
    {
        //DriverCollisionHandler.instance?.EnableDirtScreen(packet.playerData);
    }
    #endregion

    #region Win Receivers
    public void DeclareWinner(DriverArrivedPacket packet)
    {
        //WinManager.instance?.DeclareWinner(packet.hasArrived, packet.playerData);
    }
    #endregion

    #region NPC Cars Receiver
    public void InstantiateNPCCar(InstantiateNPCCarPacket packet)
    {
        CarNPCSpawner.instance?.InstantiateNpcCar(packet.indexNumber, packet.id, packet.hasSpawned);
    }
    
    public void EnableAndUpdateNPCCar(CarNPCTransformPacket packet)
    {
        CarNPCSpawner.instance?.UpdateCarPropertiesOnTheList(packet.id, packet.pos, packet.rot);
    }

    public void DisableNPCCar(DisableNPCCarPacket packet)
    {
        CarNPCSpawner.instance?.DisableNPCCar(packet.id, packet.disable);
    }
    #endregion

}
