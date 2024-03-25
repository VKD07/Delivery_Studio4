using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandlePackets : MonoBehaviour
{
    //reading the packets welcome
    //Make sure to read the write order of the packets
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int id = packet.ReadInt();

        Debug.Log($"Message from the server: {msg}");
        ClientManager.instance.myId = id;

        //Send welcome receive packet
        SendPackets.WelcomeReceived();
    }

    #region Lobby Packets
    public static void ReceiveJoinLobby(Packet packet)
    {
        PlayerLobbyManager.instance?.UpdatePlayerListAndSendNameToNetwork(packet.ReadString());
    }

    public static void ReceiveTeamAndRole(Packet packet)
    {
        LobbyUIManager.instance?.UpdateLobbyUIManager(packet.ReadInt(), (GameRole)packet.ReadInt(), packet.ReadString());
    }

    public static void ReceiveChangeTeam(Packet packet)
    {
        LobbyUIManager.instance?.UpdateChangedRolesFromNetwork(packet.ReadInt(), (GameRole)packet.ReadInt(), packet.ReadString());
    }

    public static void ReceiveStartGame(Packet packet)
    {
        LobbyUIManager.instance?.ReceivePacketIfGameHasStarted();
    }

    #endregion

    #region Driver Packets
    public static void ReceiveOtherPlayerCarProperties(Packet packet)
    {
        Vector3 carPos = packet.ReadVector3();
        Quaternion carRot = packet.ReadQuaternion();
        float wheelSpeed = packet.ReadInt();
        Quaternion flWheelRot = packet.ReadQuaternion();
        Quaternion frWheelRot = packet.ReadQuaternion();
        NetworkPlayerManager.instance?.SetEnemyProperties(carPos, carRot, wheelSpeed, flWheelRot, frWheelRot);
    }

    public static void ReceiveSpawnedCar(Packet packet)
    {
        int startLocaIndex = packet.ReadInt();
        int pointIndex = packet.ReadInt();
        Vector3 spawnPos = packet.ReadVector3();
        NetworkPlayerManager.instance?.SpawnEnemyPlayer(spawnPos, startLocaIndex, pointIndex);
    }


    public static void ReceiveDeliveryLocation(Packet packet)
    {
        LocationHandler.instance?.SetDeliveryLocation(packet.ReadString());
    }

    public static void ReceiveTimer(Packet packet)
    {
        TimerManager.instance?.UpdateTimer(packet.ReadString());
    }

    public static void ReceiveChosenPackage(Packet packet)
    {
        string packageName = packet.ReadString();
        int packageIndex = packet.ReadInt();
        int tagIndex = packet.ReadInt();

        CustomerDialougeManager.instance?.EnableAndSetCustomerDialouge(packageName, packageIndex, tagIndex);
    }

    public static void ReceiveWinPacket(Packet packet)
    {
        WinManager.instance?.DeclareWinner(packet.ReadInt());
    }
    #endregion

    #region Navigator Packets
    public static void ReceiveDriverCollision(Packet packet)
    {
        DriverCollisionHandler.instance?.TriggerRandomMapRotation();
    }
    public static void ReceiveDirtCollision(Packet packet)
    {
        DriverCollisionHandler.instance?.EnableDirtScreen();
    }
    public static void ReceiveDriverArrived(Packet packet)
    {
        NavCustomerPackage.instance?.EnablePackageUI();
    }
    public static void ReceiveIncorrectPackage(Packet packet)
    {
        CustomerDialougeManager.instance?.EnableAngryCustomer();
    }
    #endregion

    #region NPC Car Packets

    public static void ReceiveSpawnedNPCCar(Packet packet)
    {
        int carIndex = packet.ReadInt();
        int id = packet.ReadInt();
        bool hasSpawned = packet.ReadBool();
        CarNPCSpawner.instance?.InstantiateNpcCar(carIndex, id, hasSpawned);
    }

    public static void ReceiveNPCCarTransform(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 pos = packet.ReadVector3();
        Quaternion rot = packet.ReadQuaternion();
        CarNPCSpawner.instance?.UpdateCarPropertiesOnTheList(id, pos, rot);
    }

    public static void ReceiveNPCToDisable(Packet packet)
    {
        int id = packet.ReadInt();
        bool disable = packet.ReadBool();
        CarNPCSpawner.instance?.DisableNPCCar(id, disable);
    }
    #endregion
}
