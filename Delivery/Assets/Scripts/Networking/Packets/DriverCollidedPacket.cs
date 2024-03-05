using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DriverCollidedPacket : BasePacket
{
    public bool hasCollided;
    public DriverCollidedPacket() { }
    public DriverCollidedPacket(bool hasCollided) : base(PacketType.DriverHasCollided)
    {
        this.hasCollided = hasCollided;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(hasCollided);
        return EndSerialize();
    }

    //reads the buffer
    public new DriverCollidedPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        hasCollided = binaryReader.ReadBoolean();
        EndDeserialize();
        return this;
    }
}
