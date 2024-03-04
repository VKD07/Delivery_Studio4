using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RearWheelAnimationPacket : BasePacket
{
   
    
    Quaternion rotation;

    public RearWheelAnimationPacket() { }
    public RearWheelAnimationPacket(Quaternion rotation)
    {
        packetType = PacketType.WheelRot;
        this.rotation = rotation;
        base.Serialize();
        Serialize();
    }

    protected new void Serialize()
    {
        binaryWriter.Write(rotation.x);
        binaryWriter.Write(rotation.y);
        binaryWriter.Write(rotation.z);
        binaryWriter.Write(rotation.w);
    }

    public new Quaternion Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();
        Quaternion rot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        return rot;
    }
}
