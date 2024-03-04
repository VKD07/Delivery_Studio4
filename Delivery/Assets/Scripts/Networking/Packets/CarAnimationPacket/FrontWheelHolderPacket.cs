using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FrontWheelHolderPacket : BasePacket
{
    Quaternion frontWheelHolderRot;

    public FrontWheelHolderPacket() { }
    public FrontWheelHolderPacket(Quaternion frontWheelHolderRot)
    {
        packetType = PacketType.FrontWheelHolderRot;
        this.frontWheelHolderRot = frontWheelHolderRot;

        base.Serialize();
        Serialize();
    }

    protected new void Serialize()
    {
        binaryWriter.Write(frontWheelHolderRot.x);
        binaryWriter.Write(frontWheelHolderRot.y);
        binaryWriter.Write(frontWheelHolderRot.z);
        binaryWriter.Write(frontWheelHolderRot.w);
    }

    public new Quaternion Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();

        Quaternion frontWheelHolderRot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        return frontWheelHolderRot;
    }
}
