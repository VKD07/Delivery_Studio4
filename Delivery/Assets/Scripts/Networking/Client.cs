using System;
using System.Net;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

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

    public delegate void PlayerTeamAndRole(TeamAndRolePacket packet);
    public event PlayerTeamAndRole onPlayerTeamAndRole;

    public delegate void OnChangeTeam(ChangeTeamPacket packet);
    public event OnChangeTeam onChangeTeam;

    public delegate void OnGameStart(GameStartPacket packet);
    public event OnGameStart onGameStart;
    #endregion

    #region NPC Car Events
    public delegate void OnNPCCarSpawn(InstantiateNPCCarPacket packet);
    public event OnNPCCarSpawn onNPCCarSpawn;

    public delegate void NPCTransform(CarNPCTransformPacket packet);
    public event NPCTransform onNPCTransform;

    public delegate void DisableNPCCar(DisableNPCCarPacket packet);
    public event DisableNPCCar OnDisableNPCCar;
    #endregion

    #region Gameplay Events
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

    public delegate void OnDirtPacket(DirtScreenPacket packet);
    public event OnDirtPacket onDirtPacket;

    public delegate void OnTimerPacket(TimerPacket packet);
    public event OnTimerPacket onTimerPacket;
    #endregion

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
    public void ConnectToServer(string ipAddress, string playerName, int teamNum, GameRole role)
    {
        playerData = new PlayerData(playerName, teamNum, role);
        //creatign a socket for the client
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //client will try to connect to the server IPadress in port 3000
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 3000));
        clientSocket.Blocking = false;
        connected = true;

        Debug.Log($"Successfully connected to {ipAddress} in port 3000");
        Debug.Log($"PlayerName: {playerData.name}");
        Debug.Log($"PartnerName: {playerData.teamNumber}");
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
                BasePacket.Reset();

                ThreadManager.ExecuteOnMainThread(() =>
                {
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
                                    onLobbyJoined(new JoinServerPacket().Deserialize(buffer));
                                    break;

                                case PacketType.TeamAndRole:
                                    onPlayerTeamAndRole(new TeamAndRolePacket().Deserialize(buffer));
                                    break;

                                case PacketType.ChangeTeam:
                                    onChangeTeam(new ChangeTeamPacket().Deserialize(buffer));
                                    break;

                                case PacketType.StartGame:
                                    onGameStart(new GameStartPacket().Deserialize(buffer));
                                    break;
                                #endregion

                                #region NPC Car Packets
                                case PacketType.SpawnNPCCar:
                                    onNPCCarSpawn(new InstantiateNPCCarPacket().Deserialize(buffer));
                                    break;

                                case PacketType.NPCCarTransform:
                                    onNPCTransform(new CarNPCTransformPacket().Deserialize(buffer));
                                    break;

                                case PacketType.DisableNPCCar:
                                    OnDisableNPCCar(new DisableNPCCarPacket().Deserialize(buffer));
                                    break;
                                #endregion

                                #region GamePlay Packets
                                case PacketType.DeliveryLocation:
                                    //if packet is delivery locations
                                    //Extract the buffer data and set the delivery location from this data
                                    try { onDeliveryAddress(new DeliveryLocationPacket().Deserialize(buffer)); } catch (Exception) { }
                                    break;

                                case PacketType.DriverArrived:
                                    onDriverArrived(new DriverArrivedPacket().Deserialize(buffer));
                                    break;

                                case PacketType.DriverHasCollided:
                                    try { onDriverCollision(new DriverCollidedPacket().Deserialize(buffer)); } catch (Exception) { }
                                    break;

                                case PacketType.SpawnEnemy:
                                    try { onEnemySpawn(new SpawnEnemyPacket().Deserialize(buffer)); } catch (Exception) { }
                                    break;

                                case PacketType.UpdateEnemyProperties:
                                    try { onMove(new EnemyPropertiesPacket().Deserialize(buffer)); } catch (Exception) { }
                                    break;

                                case PacketType.DirtScreen:
                                    try { onDirtPacket(new DirtScreenPacket().Deserialize(buffer)); } catch (Exception) { }
                                    break;

                                case PacketType.Timer:
                                    try { onTimerPacket(new TimerPacket().Deserialize(buffer)); } catch (Exception) { }
                                    break;
                                    #endregion
                            }
                        }
                    }
                });
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
