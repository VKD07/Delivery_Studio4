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
        int spawnIndex = packet.ReadInt();
        Vector3 spawnPos = packet.ReadVector3();
        NetworkPlayerManager.instance?.SpawnEnemyPlayer(spawnPos, spawnIndex);
    }

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
        CarNPCSpawner.instance?.DisableNPCCar(id,disable);
    }
}
