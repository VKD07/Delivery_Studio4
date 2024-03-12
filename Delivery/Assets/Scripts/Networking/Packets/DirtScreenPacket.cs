using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtScreenPacket : BasePacket
{
    public DirtScreenPacket()
    {
    }

    public DirtScreenPacket(PlayerData playerData): base(PacketType.DirtScreen, playerData)
    {
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        return EndSerialize();
    }

    public new DirtScreenPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        EndDeserialize();
        return this;
    }
}
