using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartPacket : BasePacket
{
    public bool gameHasStarted;
    public GameStartPacket()
    {
        gameHasStarted = false;
    }
    public GameStartPacket(bool gameHasStarted, PlayerData playerData) :
        base(PacketType.StartGame, playerData)
    {
        this.gameHasStarted = gameHasStarted;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(gameHasStarted);
        return EndSerialize();
    }

    public new GameStartPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        gameHasStarted = binaryReader.ReadBoolean();
        EndDeserialize();
        return this;
    }
}
