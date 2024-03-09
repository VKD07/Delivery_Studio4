using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnEnemyPacket : BasePacket
{
    public Vector3 pos;
    public int spawnIndex;

    public SpawnEnemyPacket(){}
    public SpawnEnemyPacket(Vector3 pos, int spawnIndex,PlayerData playerData) 
        : base(PacketType.SpawnEnemy, playerData)
    {
        this.pos = pos;
        this.spawnIndex = spawnIndex;
    }

    public byte [] Serialize()
    {
        BeginSerialize();
        //writing position
        binaryWriter.Write(pos.x);
        binaryWriter.Write(pos.y);
        binaryWriter.Write(pos.z);
        binaryWriter.Write(spawnIndex);
        return EndSerialize();
    }

    //reads string from buffer
    public new SpawnEnemyPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        pos = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        spawnIndex = binaryReader.ReadInt32();
        EndDeserialize();
        return this;
    }

    public override void Dispose() => base.Dispose();
}
