using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAndRolePacket : BasePacket
{
    public TeamAndRolePacket()
    {
    }

    public TeamAndRolePacket(PlayerData playerData)
        : base(PacketType.TeamAndRole, playerData) { 
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        return EndSerialize();
    }

    public new TeamAndRolePacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        EndDeserialize();
        return this;
    }
}
