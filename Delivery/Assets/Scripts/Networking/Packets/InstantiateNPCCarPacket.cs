

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateNPCCarPacket : BasePacket
{
    public int indexNumber;
    public int id;
    public bool hasSpawned;

    public InstantiateNPCCarPacket()
    {
        indexNumber = 0;
    }

    public InstantiateNPCCarPacket(int poolNum, int id, bool hasSpawned, PlayerData playerData) : base(PacketType.SpawnNPCCar, playerData)
    {
        this.indexNumber = poolNum;
        this.id = id;
        this.hasSpawned = hasSpawned;
    }


    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(indexNumber);
        binaryWriter.Write(id);
        binaryWriter.Write(hasSpawned);
        return EndSerialize();
    }

    public new InstantiateNPCCarPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        indexNumber = binaryReader.ReadInt32();
        id = binaryReader.ReadInt32();
        hasSpawned = binaryReader.ReadBoolean();
        EndDeserialize();
        return this;
    }
}
