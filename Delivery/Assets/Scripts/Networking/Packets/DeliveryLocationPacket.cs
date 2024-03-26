using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class DeliveryLocationPacket : BasePacket
{
   public string buildingName { get; private set; }
   public DeliveryLocationPacket() { }
   public DeliveryLocationPacket(string buildingName, PlayerData playerData) 
        : base(PacketType.DeliveryLocation, playerData)
    {
        this.buildingName = buildingName;
    }

    public byte [] Serialize()
    {
        BeginSerialize();
        binaryWriter.Write(buildingName);
        return EndSerialize();
    }

    //reads string from buffer
    public new DeliveryLocationPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        buildingName = binaryReader.ReadString();
        EndDeserialize();
        return this;
    }
}
