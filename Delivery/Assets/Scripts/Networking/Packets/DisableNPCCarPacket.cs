using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNPCCarPacket : BasePacket
{
    public int id;
    public bool disable;

    public DisableNPCCarPacket()
    {
        disable = false;
    }
    public DisableNPCCarPacket(int id, bool disable, PlayerData playerData) : base(PacketType.DisableNPCCar, playerData)
    {
        this.id = id;
        this.disable = disable;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(id);
        binaryWriter.Write(disable);
        return EndSerialize();
    }

    public new DisableNPCCarPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        id = binaryReader.ReadInt32();
        disable = binaryReader.ReadBoolean();
        EndDeserialize();
        return this;
    }
}
