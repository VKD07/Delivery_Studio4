using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum PacketType
{
    None = 0,
    Message,

    //Game play packets
    DeliveryLocation,
    DriverArrived,
    DriverHasCollided,
    DirtScreen,
    Timer,

    //Sending PlayerPos
    SpawnEnemy,
    UpdateEnemyProperties,

    //Lobby Packet
    HasJoinedLobby,
    TeamAndRole,
    ChangeTeam,
    StartGame,

    //NPC Car Packet
    SpawnNPCCar,
    NPCCarTransform,
    DisableNPCCar
}

public class BasePacket : IDisposable
{
    //for binary writing
    protected MemoryStream sms; //protected
    protected BinaryWriter binaryWriter; //protected

    //for binary reading
    protected MemoryStream dms; //protected
    protected BinaryReader binaryReader; // protected
    public PacketType packetType { get; protected set; }

    public int packetSize { get; private set; }
    public static int currentBufferPosition;

    public PlayerData playerData { get; private set; }

    public BasePacket()
    {
        packetType = PacketType.None;
        playerData = null;
    }

    public BasePacket(PacketType packetType, PlayerData playerData)
    {
        this.packetType = packetType;
        this.playerData = playerData;
    }

    protected void BeginSerialize()
    {
        sms = new MemoryStream();
        binaryWriter = new BinaryWriter(sms);
        binaryWriter.Write((int)packetType);

        //writing the player data
        binaryWriter.Write(playerData.name);
        binaryWriter.Write(playerData.teamNumber);
        binaryWriter.Write((int)playerData.role);
    }

    protected byte[] EndSerialize()
    {
        packetSize = (int)sms.Length + 4; //setting the packet size to tell how much buffer size we have
        binaryWriter.Write(packetSize); //writing the packet size
        return sms.ToArray();
    }

    public BasePacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        return this;
    }

    protected void BeginDeserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        dms.Seek(currentBufferPosition, SeekOrigin.Begin); //making sure to read in a position starting from the index where the unread bytes are
        binaryReader = new BinaryReader(dms);
        packetType = (PacketType)binaryReader.ReadInt32();

        //reading player data
        playerData = new PlayerData(binaryReader.ReadString(), binaryReader.ReadInt32(), (GameRole)binaryReader.ReadInt32());
    }

    protected void EndDeserialize()
    {
        packetSize = binaryReader.ReadInt32();
        currentBufferPosition += packetSize;
    }

    public static bool DataRemainingInBuffer(int bufferSize)
    {
        if (currentBufferPosition < bufferSize)
        {
            return true;
        }
        currentBufferPosition = 0;
        return false;
    }

    public static void Reset()
    {
        currentBufferPosition = 0;
    }

    bool disposedValue = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                sms?.Dispose();
                binaryWriter?.Dispose();
                binaryReader?.Dispose();
                bool disposedSuccessfully = (sms == null && binaryWriter == null && binaryReader == null);
                if (disposedSuccessfully)
                {
                    Console.WriteLine("Objects were successfully disposed.");
                }
                else
                {
                    Console.WriteLine("Dispose failed for one or more objects.");
                }
            }
            disposedValue = true;
        }
    }
}