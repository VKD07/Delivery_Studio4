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
        LobbyManager.instance?.UpdatePlayerListAndSendNameToNetwork(packet.ReadString());
        //PlayerLobbyManager.instance?.UpdatePlayerListAndSendNameToNetwork(packet.ReadString());
    }

    public static void ReceiveLobbyRequest(Packet packet)
    {
        LobbyMode modeReceived = (LobbyMode)packet.ReadInt();

        switch (modeReceived)
        {
            case LobbyMode.Duo:
                LobbyManager.instance?.EnableDuoLobby();
                break;
            case LobbyMode.TwoVTwo:
                LobbyManager.instance?.EnableTwoVTwoLobby();
                break;
        }

        LobbyManager.instance?.SendJoinLobbyPacket();
    }

    public static void ReceiveTeamAndRole(Packet packet)
    {
        LobbyManager.instance?.UpdateLobbyUIManager(packet.ReadInt(), (GameRole)packet.ReadInt(), packet.ReadString(), (LobbyMode)packet.ReadInt());
    }

    public static void ReceiveChangeTeam(Packet packet)
    {
        LobbyManager.instance?.UpdateChangedRolesFromNetwork(packet.ReadInt(), (GameRole)packet.ReadInt(), packet.ReadString(), (LobbyMode)packet.ReadInt());
    }

    public static void ReceiveStartGame(Packet packet)
    {
        LobbyManager.instance?.ReceivePacketIfGameHasStarted();
    }

    #endregion

    #region Driver Packets
    public static void ReceiveOtherPlayerCarProperties(Packet packet)
    {
        Vector3 carPos = packet.ReadVector3();
        Quaternion carRot = packet.ReadQuaternion();
        float wheelSpeed = packet.ReadFloat();
        Quaternion flWheelRot = packet.ReadQuaternion();
        Quaternion frWheelRot = packet.ReadQuaternion();
        NetworkPlayerManager.instance?.SetEnemyProperties(carPos, carRot, wheelSpeed, flWheelRot, frWheelRot);
    }

    public static void ReceiveSpawnedCar(Packet packet)
    {
        int startLocaIndex = packet.ReadInt();
        int pointIndex = packet.ReadInt();
        Vector3 spawnPos = packet.ReadVector3();
        string userName = packet.ReadString();
        NetworkPlayerManager.instance?.SpawnEnemyPlayer(spawnPos, startLocaIndex, pointIndex, userName);
    }

    public static void ReceiveCarMalfunction(Packet packet)
    {
        NetworkPlayerManager.instance?.SetSmokeVFX(packet.ReadBool());
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

    public static void ReceiveOtherPlayerAudio(Packet packet)
    {
        float pitch = packet.ReadFloat();
        NetworkPlayerManager.instance?.SetEnemyAudioProperties(pitch);
    }

    public static void ReceiveCarScreechingAudio(Packet packet)
    {
        NetworkPlayerManager.instance?.PlayCarScreechingAudio(packet.ReadBool());
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

    #region Rating Leaderboard Packets

    static List<string> playerNames = new List<string>();
    static List<int> ratings = new List<int>();
    static bool newData;

    public static void ReceiveOverAllRating(Packet packet)
    {
        ClearData();

        string playerName = packet.ReadString();
        int rating = packet.ReadInt();

        int index = playerNames.IndexOf(playerName);
        if (index != -1)
        {
            ratings[index] = rating;
        }
        else
        {
            playerNames.Add(playerName);
            ratings.Add(rating);
        }

        RatingUIManager.instance.ReceiveNamesAndRatings(playerNames.ToArray(), ratings.ToArray());
    }

    public static void ReceiveLeaderboard(Packet packet)
    {
        Debug.Log("Successfully received Leaderboard");
        LeaderboardUIManager.instance.partnerHasSent = true;
        LeaderboardUIManager.instance?.SetLeaderBoardUI(packet.ReadInt(), packet.ReadString(), packet.ReadString());
    }

    #endregion

    /// <summary>
    /// To Ensure that each time new game is created all the rating data is cleared
    /// </summary>
    static void ClearData()
    {
        if (!newData)
        {
            newData = true;
            playerNames.Clear();
            ratings.Clear();
        }
    }
}
