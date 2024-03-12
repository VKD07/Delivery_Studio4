using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSender : MonoBehaviour
{
    public static NetworkSender instance { get; private set; }
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

    #region Driver SentPackets

    public void SendSpawnEnemyPacket(Vector3 spawnPos, int spawnIndex)
    {
        using (SpawnEnemyPacket packet = new SpawnEnemyPacket(transform.position, spawnIndex, client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void SendCarProperties(Vector3 pos, Quaternion rot, float wheelSpeed, Quaternion flWheelHolder, Quaternion frWheelHolder)
    {
        using (EnemyPropertiesPacket packet = new EnemyPropertiesPacket(pos, rot,
                    wheelSpeed, flWheelHolder, frWheelHolder, client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void SendCollisionPacket()
    {
        using (DriverCollidedPacket packet = new DriverCollidedPacket(true, client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void SendDirtPacket()
    {
        using (DirtScreenPacket packet = new DirtScreenPacket(client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void SendNetworkDriverArrived()
    {
        using (DriverArrivedPacket packet = new DriverArrivedPacket(true, client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }
    #endregion

    #region Navigator SentPackets
    public void SendDeliveryLocationToDriver(string chosenBuilding)
    {
        using (DeliveryLocationPacket packet = new DeliveryLocationPacket(chosenBuilding, client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void SendCurrentTimer(string currentTime)
    {
        using(TimerPacket packet = new TimerPacket(currentTime, client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }
    #endregion

    #region LobbySentPackets
    public void SendLobbyJoinPacket()
    {
        using (JoinServerPacket packet = new JoinServerPacket(client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void SendRoleAndTeamPacket()
    {
        using (TeamAndRolePacket packet = new TeamAndRolePacket(client.playerData))
        {
            client?.SendPacket(packet.Serialize());
        }
    }

    public void SendChangeTeamPacket()
    {
        using (ChangeTeamPacket packet = new ChangeTeamPacket(client.playerData))
        {
            client?.SendPacket(packet.Serialize());
        }
    }

    public void SendStartGamePacket()
    {
        using (GameStartPacket packet = new GameStartPacket(true, client.playerData))
        {
            client?.SendPacket(packet.Serialize());
        }
    }
    #endregion

    #region NPC Cars Packets
    public void InstantiateNPCCar(int randomCarIndex, int id, bool hasSpawned)
    {
        using (InstantiateNPCCarPacket packet = new InstantiateNPCCarPacket(randomCarIndex, id, hasSpawned, client.playerData))
        {
            client?.SendPacket(packet.Serialize());
        }
    }

    public void SendNPCCarTransform(int id, Vector3 pos, Quaternion rot)
    {
        using (CarNPCTransformPacket packet = new CarNPCTransformPacket(id, pos, rot, client.playerData))
        {
            client?.SendPacket(packet.Serialize());
        }
    }

    public void DisableNPCar(int id)
    {
        using (DisableNPCCarPacket packet = new DisableNPCCarPacket(id, true, client.playerData))
        {
            client?.SendPacket(packet.Serialize());
        }
    }
    #endregion
}
