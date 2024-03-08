using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinServerPacket : BasePacket
{
    public JoinServerPacket()
    {
    }

    public JoinServerPacket(PlayerData playerData)
        : base(PacketType.HasJoinedLobby, playerData)
    {
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        return EndSerialize();
    }

    public new JoinServerPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        EndDeserialize();
        return this;
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
