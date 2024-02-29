using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;

    Socket clientSocket;
    bool connected;
    public delegate void OnMessageReceived(string message);
    public event OnMessageReceived onMessageReceived;

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


    public void ConnectToServer(string ipAddress)
    {
        //creatign a socket for the client
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //client will try to connect to the server IPadress in port 3000
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 3000));
        clientSocket.Blocking = false;
        connected = true;

        Debug.Log($"Successfully connected to {ipAddress} in port 3000");
    }

    private void Update()
    {
        ReceiveDataBuffer();
    }

    private void ReceiveDataBuffer()
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

                //reading the packet type enum from the buffer
                BasePacket basePacket = new BasePacket().Deserialize(buffer);

                switch (basePacket.packetType)
                {
                    case PacketType.DeliveryLocation:

                        //if packet is delivery location
                        //Extract the buffer data and set the delivery location from this data
                        LocationHandler.instance.SetDeliveryLocation(new DeliveryLocationPacket().Deserialize(buffer));
                        Debug.Log($"Delivery Location set: {new DeliveryLocationPacket().Deserialize(buffer)}");

                        break;

                    case PacketType.DriverArrived:
                        WinManager.instance.DeclareWinner(new DriverArrivedPacket().Deserialize(buffer));
                        Debug.Log($"Driver has arrived {new DriverArrivedPacket().Deserialize(buffer)}");
                        break;

                    case PacketType.DriverHasCollided:
                        DriverCollisionHandler.instance.TriggerRandomMapRotation(new DriverCollidedPacket().Deserialize(buffer));
                        Debug.Log($"Driver has collided {new DriverArrivedPacket().Deserialize(buffer)}");
                        break;
                }
            }
            catch (SocketException ex)
            {

            }
        }
    }

    public void SendPacket(BasePacket packet)
    {
        try
        {
            clientSocket.Send(packet.ByteBuffer);
        }
        catch (SocketException ex)
        {
            Debug.Log("Failed to get BUffer");
        }
    }


    //public new void SendMessage(string message)
    //{
    //    try
    //    {
    //        MessagePacket packet = new MessagePacket(message);
    //        //sending a packet type int
    //        //sending a message string
    //        clientSocket.Send(packet.ByteBuffer);
    //    }
    //    catch (SocketException ex)
    //    {
    //        Debug.Log("Failed to get BUffer");
    //    }
    //}
}
