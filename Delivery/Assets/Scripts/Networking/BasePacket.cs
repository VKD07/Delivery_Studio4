using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum PacketType
{
    None = 0,
    Message = 1,

    //Game play packets
    DeliveryLocation = 2,
    DriverArrived = 3,
    DriverHasCollided = 4,

    //Sending PlayerPos
    SpawnEnemy = 5,
    UpdateEnemyProperties = 6,

    //Lobby Packet
    HasJoinedLobby = 7,
    TeamAndRole = 8,
    ChangeTeam = 9,
    StartGame = 10,
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

    public virtual void Dispose()
    {
        sms?.Dispose();
        binaryWriter?.Dispose();
        dms?.Dispose();
        binaryReader?.Dispose();
    }
}