using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DriverArrivedPacket : BasePacket
{
    public bool hasArrived { get; private set; }

    public DriverArrivedPacket() { }
    public DriverArrivedPacket(bool hasArrived) : base(PacketType.DriverArrived)
    {
        this.hasArrived = hasArrived;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(hasArrived);
        return EndSerialize();
    }

    public new DriverArrivedPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        hasArrived = binaryReader.ReadBoolean();
        EndDeserialize();
        return this;
    }
}
