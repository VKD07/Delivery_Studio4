using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum PacketType
{
    None = 0,
    Message = 1,
    DeliveryLocation = 2,
    DriverArrived = 3,
    DriverHasCollided = 4,

    SpawnEnemy = 5,
    UpdateEnemyPos = 6,
    UpdateEnemyRot = 7,
}

public class BasePacket
{
    //for binary writing
    public MemoryStream sms { get; protected set; }
    public BinaryWriter binaryWriter { get; protected set; }

    //for binary reading
    public MemoryStream dms { get; protected set; }

    public BinaryReader binaryReader { get; protected set; }

    public PacketType packetType { get; protected set; }

    /// <summary>
    /// Writes data and convert it into a buffer
    /// </summary>
    protected void Serialize()
    {
        sms = new MemoryStream();
        binaryWriter = new BinaryWriter(sms);
        binaryWriter.Write((int)packetType);
    }

    /// <summary>
    /// converts buffer into primitive data
    /// receives packet type from byte
    /// </summary>
    /// <param name="buffer"></param>
    public BasePacket Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        packetType = (PacketType)binaryReader.ReadInt32();
        return this;
    }

    public byte[] ByteBuffer => sms.ToArray();
}