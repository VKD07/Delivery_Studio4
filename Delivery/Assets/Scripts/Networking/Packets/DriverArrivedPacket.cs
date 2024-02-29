using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DriverArrivedPacket : BasePacket
{
    bool hasArrived;

    public DriverArrivedPacket() { }
    public DriverArrivedPacket(bool hasArrived)
    {
        packetType = PacketType.DriverArrived;
        this.hasArrived = hasArrived;
        base.Serialize();
        Serialize();
    }

    protected new void Serialize()
    {
        binaryWriter.Write(hasArrived);
    }

    //reads string from buffer
    public new bool Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();
        bool hasArrived = binaryReader.ReadBoolean();
        return hasArrived;
    }
}
