using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SendPackets : MonoBehaviour
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        ClientManager.instance?.tcp.SendData(packet);
    }

    //packets
    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.welcome))
        {
            packet.Write(ClientManager.instance.playerData.name);
            SendTCPData(packet);
        }
    }

    #region Lobby Packets
    public static void SendJoinLobbyPacket(string playerName)
    {
        using (Packet packet = new Packet((int)ClientPackets.joinLobby))
        {
            packet.Write(playerName);
            SendTCPData(packet);
        }
    }

    public static void SendTeamAndRole(int teamNum, int gameRole, string playerName)
    {
        using (Packet packet = new Packet((int)ClientPackets.teamAndRole))
        {
            packet.Write(teamNum);
            packet.Write(gameRole);
            packet.Write(playerName);
            SendTCPData(packet);
        }
    }

    public static void SendTeamChange(int teamNum, int gameRole, string playerName)
    {
        using (Packet packet = new Packet((int)ClientPackets.teamChange))
        {
            packet.Write(teamNum);
            packet.Write(gameRole);
            packet.Write(playerName);
            SendTCPData(packet);
        }
    }

    public static void SendStartGamePacket()
    {
        using (Packet packet = new Packet((int)ClientPackets.startGame))
        {
            SendTCPData(packet);
        }
    }

    public static void SendPlayerData(string name, int teamNum, int role)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerData))
        {
            packet.Write(name);
            packet.Write(teamNum);
            packet.Write(role);
            SendTCPData(packet);
        }
    }

    #endregion

    #region Driver Packets
    public static void SpawnCar(Vector3 pos, int randomIndex)
    {
        using (Packet packet = new Packet((int)ClientPackets.spawnCar))
        {
            packet.Write(randomIndex);
            packet.Write(pos);
            SendTCPData(packet);
        }
    }

    public static void SendCarProperties(Vector3 pos, Quaternion rot, float wheelSpeed,
                                         Quaternion flWheelHolderRot, Quaternion frWheelHolderRot)
    {
        using (Packet packet = new Packet((int)ClientPackets.carProperties))
        {
            //pos
            packet.Write(pos);
            //car rot
            packet.Write(rot);
            //wheel speed
            packet.Write(wheelSpeed);
            //FlWheelRot
            packet.Write(flWheelHolderRot);
            //FrWheelRot
            packet.Write(frWheelHolderRot);
            SendTCPData(packet);
        }
    }

    public static void SendDriverCollision()
    {
        using (Packet packet = new Packet((int)ClientPackets.driverCollided))
        {
            SendTCPData(packet);
        }
    }

    public static void SendDirtCollision()
    {
        using (Packet packet = new Packet((int)ClientPackets.dirtCollision))
        {
            SendTCPData(packet);
        }
    }

    public static void SendDriverArrived(int teamNumber)
    {
        using (Packet packet = new Packet((int)ClientPackets.driverArrived))
        {
            SendTCPData(packet);
        }
    }
    #endregion

    #region Navigator Packets
    public static void SendDeliveryLocation(string chosenBuilding)
    {
        using (Packet packet = new Packet((int)ClientPackets.deliveryAddress))
        {
            packet.Write(chosenBuilding);
            SendTCPData(packet);
        }
    }

    public static void SendTimer(string timer)
    {
        using (Packet packet = new Packet((int)ClientPackets.timer))
        {
            packet.Write(timer);
            SendTCPData(packet);
        }
    }

    public static void SendChosenPackageProperties(string packageName, int packageIndex, int tagIndex)
    {
        using (Packet packet = new Packet((int)ClientPackets.chosenPackage))
        {
            packet.Write(packageName);
            packet.Write(packageIndex);
            packet.Write(tagIndex);
            SendTCPData(packet);
        }
    }

    public static void SendWinPacket(int teamNumber)
    {
        using (Packet packet = new Packet((int)ClientPackets.win))
        {
            packet.Write(teamNumber);
            SendTCPData(packet);
        }
    }
    #endregion

    #region NPC Car Packets
    public static void SendNPCCars(int randomCarIndex, int id, bool val)
    {
        using (Packet packet = new Packet((int)ClientPackets.npcSpawn))
        {
            packet.Write(randomCarIndex);
            packet.Write(id);
            packet.Write(val);
            SendTCPData(packet);
        }
    }

    public static void SendNPCCarTransform(int id, Vector3 pos, Quaternion rot)
    {
        using (Packet packet = new Packet((int)ClientPackets.npcTransform))
        {
            packet.Write(id);
            packet.Write(pos);
            packet.Write(rot);
            SendTCPData(packet);
        }
    }

    public static void DisableNPCCar(int id)
    {
        using (Packet packet = new Packet((int)ClientPackets.npcDisable))
        {
            packet.Write(id);
            packet.Write(false);
            SendTCPData(packet);
        }
    }
    #endregion
}
