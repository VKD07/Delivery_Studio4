using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnEnemyPacket : BasePacket
{
    public Vector3 pos;

    public SpawnEnemyPacket(){}
    public SpawnEnemyPacket(Vector3 pos): base(PacketType.SpawnEnemy)
    {
        this.pos = pos;
    }

    public byte [] Serialize()
    {
        BeginSerialize();
        //writing position
        binaryWriter.Write(pos.x);
        binaryWriter.Write(pos.y);
        binaryWriter.Write(pos.z);
        return EndSerialize();
    }

    //reads string from buffer
    public new SpawnEnemyPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        pos = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        EndDeserialize();
        return this;
    }

    public override void Dispose() => base.Dispose();
}
