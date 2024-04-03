using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 7777;
    public int myId = 0;
    public TCP tcp;
    public PlayerData playerData;
    private bool isConnected = false;
    //delegate events for the packet
    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>();

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

        playerData = new PlayerData("", 0, GameRole.None);
    }

    private void Start()
    {
        tcp = new TCP();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer(string Ipaddress)
    {
        InitializeClientData();
        isConnected = true;
        tcp.Connect(Ipaddress);
    }

    public void SetPlayerName(string playerName)
    {
        playerData.name = playerName;
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.welcome, HandlePackets.Welcome},

            {(int)ServerPackets.joinLobby, HandlePackets.ReceiveJoinLobby},
            {(int)ServerPackets.teamAndRole, HandlePackets.ReceiveTeamAndRole},
            {(int)ServerPackets.teamChange, HandlePackets.ReceiveChangeTeam},
            {(int)ServerPackets.startGame, HandlePackets.ReceiveStartGame},
            {(int)ServerPackets.lobbyRequest, HandlePackets.ReceiveLobbyRequest},

            {(int)ServerPackets.spawnCar, HandlePackets.ReceiveSpawnedCar},
            {(int)ServerPackets.carProperties, HandlePackets.ReceiveOtherPlayerCarProperties},
            {(int)ServerPackets.driverCollided, HandlePackets.ReceiveDriverCollision},
            {(int)ServerPackets.dirtCollision, HandlePackets.ReceiveDirtCollision},
            {(int)ServerPackets.timer, HandlePackets.ReceiveTimer},
            {(int)ServerPackets.carMalfunction, HandlePackets.ReceiveCarMalfunction},

            {(int)ServerPackets.carAudio, HandlePackets.ReceiveOtherPlayerAudio},
            {(int)ServerPackets.screechingAudio, HandlePackets.ReceiveCarScreechingAudio},

            {(int)ServerPackets.deliveryAddress, HandlePackets.ReceiveDeliveryLocation},
            {(int)ServerPackets.driverArrived, HandlePackets.ReceiveDriverArrived},
            {(int)ServerPackets.chosenPackage, HandlePackets.ReceiveChosenPackage},
            {(int)ServerPackets.win, HandlePackets.ReceiveWinPacket},
            {(int)ServerPackets.wrongPackage, HandlePackets.ReceiveIncorrectPackage},

            {(int)ServerPackets.npcSpawn, HandlePackets.ReceiveSpawnedNPCCar},
            {(int)ServerPackets.npcTransform, HandlePackets.ReceiveNPCCarTransform},
            {(int)ServerPackets.npcDisable, HandlePackets.ReceiveNPCToDisable},
        };
    }

    public class TCP
    {
        public TcpClient client;
        Packet receivedData;
        NetworkStream stream;
        byte[] receiveBuffer; 

        public void Connect(string ip)
        {
            client = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize,
            };

            receiveBuffer = new byte[dataBufferSize];
            client.BeginConnect(ip, instance.port, ConnectCallBack, client);
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (client != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Error sending data to player via TCP");
                Disconnect();
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            client.EndConnect(ar);

            if (!client.Connected)
            {
                return;
            }

            stream = client.GetStream();

            receivedData = new Packet();

            //putting the byte data to receive buffer array starting from index  0
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int byteLength = stream.EndRead(ar);
                if (byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }
                byte[] newData = new byte[byteLength];
                Array.Copy(receiveBuffer, newData, byteLength);

                receivedData.Reset(HandleData(newData));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] data)
        {
            int packetLength = 0;

            receivedData.SetBytes(data);

            //this means we have the integer packet, the first data we wanted to send
            if (receivedData.UnreadLength() >= 4)
            {
                packetLength = receivedData.ReadInt();

                if (packetLength <= 0)
                {
                    return true;
                }
            }

            //that means that there are still remaining bytes that we want to read
            //other than the integer
            while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
            {
                byte[] packetBytes = receivedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        packetHandlers[packetId](packet);
                    }
                });
                packetLength = 0;

                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();

                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        private void Disconnect()
        {
            instance.Disconnect();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            client = null;
        }
    }

    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.client.Close();

            Debug.Log("Disconnected from server.");
        }
    }
}
