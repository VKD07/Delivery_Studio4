using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeamPacket : BasePacket
{
    public ChangeTeamPacket()
    {
    }

    public ChangeTeamPacket(PlayerData playerData)
        : base(PacketType.ChangeTeam, playerData)
    {
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        return EndSerialize();
    }

    public new ChangeTeamPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        EndDeserialize();
        return this;
    }
}
