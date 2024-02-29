using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DriverCollidedPacket : BasePacket
{
    bool hasCollided;
    public DriverCollidedPacket() { }
    public DriverCollidedPacket(bool hasCollided)
    {
        packetType = PacketType.DriverHasCollided;
        this.hasCollided = hasCollided;
        base.Serialize();
        Serialize();

    }

    protected new void Serialize()
    {
        binaryWriter.Write(hasCollided);
    }

    //reads the buffer
    public new bool Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();
        bool hasCollided = binaryReader.ReadBoolean();
        return hasCollided;
    }
}
