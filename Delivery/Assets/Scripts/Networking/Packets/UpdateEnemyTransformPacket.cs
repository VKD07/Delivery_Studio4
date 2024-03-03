using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class UpdateEnemyTransformPacket : BasePacket
{
    Vector3 pos;
    Quaternion rot;

    public UpdateEnemyTransformPacket() { }
    public UpdateEnemyTransformPacket(Vector3 pos, Quaternion rotation)
    {
        packetType = PacketType.UpdateEnemyPos;

        this.pos = pos;
        this.rot = rotation;

        base.Serialize();
        Serialize();
    }

    protected new void Serialize()
    {
        //writing position
        binaryWriter.Write(pos.x);
        binaryWriter.Write(pos.y);
        binaryWriter.Write(pos.z);

        //writing rotation
        binaryWriter.Write(rot.x);
        binaryWriter.Write(rot.y);
        binaryWriter.Write(rot.z);
        binaryWriter.Write(rot.w);
    }

    //reads string from buffer
    public new (Vector3 pos, Quaternion rot) Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();
        Vector3 pos = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());

        Quaternion rot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        return (pos, rot);
    }
}
