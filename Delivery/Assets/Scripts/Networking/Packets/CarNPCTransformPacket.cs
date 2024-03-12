using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNPCTransformPacket : BasePacket
{
    public int id;
    public Vector3 pos;
    public Quaternion rot;

    public CarNPCTransformPacket()
    {
        id = 0;
        pos = Vector3.zero;
        rot = Quaternion.identity;
    }

    public CarNPCTransformPacket(int id, Vector3 pos, Quaternion rot, PlayerData playerData) : base(PacketType.NPCCarTransform, playerData)
    {
        this.id = id;
        this.pos = pos;
        this.rot = rot;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(id);

        binaryWriter.Write(pos.x);
        binaryWriter.Write(pos.y);
        binaryWriter.Write(pos.z);

        binaryWriter.Write(rot.x);
        binaryWriter.Write(rot.y);
        binaryWriter.Write(rot.z);
        binaryWriter.Write(rot.w);
        return EndSerialize();
    }

    public new CarNPCTransformPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        id = binaryReader.ReadInt32();
        pos = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        rot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        EndDeserialize();
        return this;
    }
}
