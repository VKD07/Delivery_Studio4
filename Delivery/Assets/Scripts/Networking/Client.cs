using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Client : MonoBehaviour
{
    public static Client instance;

    Socket clientSocket;
    bool connected;

    public delegate void OnMessageReceived(string message);
    public event OnMessageReceived onMessageReceived;

    #region Lobby Events
    public delegate void OnLobbyJoined(JoinServerPacket packet);
    public event OnLobbyJoined onLobbyJoined;
    #endregion

    public delegate void SendDeliveryAddress(DeliveryLocationPacket packet);
    public event SendDeliveryAddress onDeliveryAddress;

    public delegate void OnDriverArrived(DriverArrivedPacket packet);
    public event OnDriverArrived onDriverArrived;

    public delegate void OnDriverCollision(DriverCollidedPacket packet);
    public event OnDriverCollision onDriverCollision;

    public delegate void OnEnemySpawn(SpawnEnemyPacket packet);
    public event OnEnemySpawn onEnemySpawn;

    public delegate void OnMove(EnemyPropertiesPacket packet);
    public event OnMove onMove;

    public PlayerData playerData;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //TODO : Input player data
    public void ConnectToServer(string ipAddress, string playerName, string partnerName, GameRole role)
    {
        Client.instance.playerData = new PlayerData(playerName, partnerName, role);
        //creatign a socket for the client
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //client will try to connect to the server IPadress in port 3000
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 3000));
        clientSocket.Blocking = false;
        connected = true;

        Debug.Log($"Successfully connected to {ipAddress} in port 3000");
        Debug.Log($"PlayerName: {playerData.name}");
        Debug.Log($"PartnerName: {playerData.partnerName}");
        Debug.Log($"Game Role: {playerData.role}");
    }

    private void Update()
    {
        ReceiveDataBuffer();
    }

    void ReceiveDataBuffer()
    {
        if (connected)
        {
            if (clientSocket.Available <= 0) return;
            try
            {
                byte[] buffer = new byte[clientSocket.Available];
                clientSocket.Receive(buffer);
                //do events and delegates instead. let Message txt update when message is being received.
                //onMessageReceived.Invoke(Encoding.ASCII.GetString(buffer));
                //if (!BasePacket.DataRemainingInBuffer(buffer.Length)) return;
                while (BasePacket.DataRemainingInBuffer(buffer.Length))
                {
                    //reading the packet type enum from the buffer
                    //BasePacket basePacket = new BasePacket().Deserialize(buffer);
                    using (BasePacket basePacket = new BasePacket().Deserialize(buffer))
                    {
                        switch (basePacket.packetType)
                        {
                            #region lobby packets
                            case PacketType.HasJoinedLobby:
                                Debug.Log("joined lobby packet received");
                                onLobbyJoined(new JoinServerPacket().Deserialize(buffer));
                                break;
                            #endregion

                            case PacketType.DeliveryLocation:
                                //if packet is delivery location
                                //Extract the buffer data and set the delivery location from this data
                                onDeliveryAddress(new DeliveryLocationPacket().Deserialize(buffer));
                                break;

                            case PacketType.DriverArrived:
                                onDriverArrived(new DriverArrivedPacket().Deserialize(buffer));
                                break;

                            case PacketType.DriverHasCollided:
                                try { onDriverCollision(new DriverCollidedPacket().Deserialize(buffer)); } catch (Exception) { }
                                break;

                            case PacketType.SpawnEnemy:
                                onEnemySpawn(new SpawnEnemyPacket().Deserialize(buffer));
                                break;

                            case PacketType.UpdateEnemyProperties:
                                onMove(new EnemyPropertiesPacket().Deserialize(buffer));
                                break;
                        }
                    }

                }
            }
            catch (SocketException ex)
            {
            }
        }
    }

    public void SendPacket(byte[] buffer)
    {
        try
        {
            clientSocket.Send(buffer);
        }
        catch (SocketException ex)
        {
            Debug.Log("Failed to send packet");
        }
    }
}
