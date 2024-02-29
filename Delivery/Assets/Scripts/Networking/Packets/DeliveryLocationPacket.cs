using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class DeliveryLocationPacket : BasePacket
{
    string buildingName;
   public DeliveryLocationPacket() { }
   public DeliveryLocationPacket(string buildingName)
    {
        packetType = PacketType.DeliveryLocation;
        this.buildingName = buildingName;

        base.Serialize();
        Serialize();
    }

    protected new void Serialize()
    {
        binaryWriter.Write(buildingName);
    }

    //reads string from buffer
    public new string Deserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        binaryReader = new BinaryReader(dms);
        int packetType = binaryReader.ReadInt32();
        string buildingName = binaryReader.ReadString();
        return buildingName;
    }
}
