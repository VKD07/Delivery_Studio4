using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WheelRotationPacket : BasePacket
{
    float wheelSpeed;
    public WheelRotationPacket() { }
    public WheelRotationPacket(float wheelSpeed)
    {
        packetType = PacketType.WheelRot;
        this.wheelSpeed = wheelSpeed;
        base.Serialize();
        Serialize();
    }

    protected new void Serialize()
    {
        binaryWriter.Write(wheelSpeed);
    }

    public new float Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();

        float speed = binaryReader.ReadSingle();
        return speed;
    }
}
