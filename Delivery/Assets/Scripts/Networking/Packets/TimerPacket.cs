using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPacket : BasePacket
{
    public string currentTime;

    public TimerPacket()
    {
        currentTime = null;
    }

    public TimerPacket(string timer, PlayerData playerData) : base(PacketType.Timer, playerData)
    {
        this.currentTime = timer;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(currentTime);
        return EndSerialize();
    } 

    public new TimerPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        currentTime = binaryReader.ReadString();
        EndDeserialize();
        return this;
    }
}
