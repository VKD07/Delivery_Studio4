using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class EnemyPropertiesPacket : BasePacket
{
    public Vector3 pos { get; private set; }
    public Quaternion rot { get; private set; }

    //wheel properties
    public float wheelSpeed { get; private set; }
    public Quaternion flWheelHolderRot { get; private set; }
    public Quaternion frWheelHolderRot { get; private set; }


    public EnemyPropertiesPacket() { }
    public EnemyPropertiesPacket(Vector3 pos, Quaternion rot, float wheelSpeed, Quaternion flWheelHolderRot, Quaternion frWheelHolderRot)
        : base(PacketType.UpdateEnemyProperties)
    {
        this.pos = pos;
        this.rot = rot;
        this.wheelSpeed = wheelSpeed;
        this.flWheelHolderRot = flWheelHolderRot;
        this.frWheelHolderRot = frWheelHolderRot;
    }

    public byte[] Serialize()
    {
        BeginSerialize();
        //writing position
        binaryWriter.Write(pos.x);
        binaryWriter.Write(pos.y);
        binaryWriter.Write(pos.z);
        //writing rot
        binaryWriter.Write(rot.x);
        binaryWriter.Write(rot.y);
        binaryWriter.Write(rot.z);
        binaryWriter.Write(rot.w);

        //Writing wheel speed
        binaryWriter.Write(wheelSpeed);

        //Writing front left wheel holder rotation
        binaryWriter.Write(flWheelHolderRot.x);
        binaryWriter.Write(flWheelHolderRot.y);
        binaryWriter.Write(flWheelHolderRot.z);
        binaryWriter.Write(flWheelHolderRot.w);

        //Writing front right wheel holder rotation
        binaryWriter.Write(frWheelHolderRot.x);
        binaryWriter.Write(frWheelHolderRot.y);
        binaryWriter.Write(frWheelHolderRot.z);
        binaryWriter.Write(frWheelHolderRot.w);

        return EndSerialize();
    }

    //reads string from buffer
    public new EnemyPropertiesPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        pos = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        rot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        wheelSpeed = binaryReader.ReadSingle();
        flWheelHolderRot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        frWheelHolderRot = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        EndDeserialize();
        return this;
    }

    public override void Dispose() => base.Dispose();
}