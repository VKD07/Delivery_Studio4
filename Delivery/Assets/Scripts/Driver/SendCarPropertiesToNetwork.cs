using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendCarPropertiesToNetwork : MonoBehaviour
{
    [SerializeField] Transform flWheelHolder, frWheelHolder;
    bool hasSpawned;

    public CarAnimation carAnimation => GetComponent<CarAnimation>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !hasSpawned)
        {
            hasSpawned = true;

            using (SpawnEnemyPacket packet = new SpawnEnemyPacket(transform.position, Client.instance.playerData))
            {
                Client.instance?.SendPacket(packet.Serialize());
            }
            //Client.instance?.SendPacket(new SpawnEnemyPacket(transform.position).Serialize());
            Debug.Log("Has spawned enemy");
        }

        if (hasSpawned)
        {
            using (EnemyPropertiesPacket packet = new EnemyPropertiesPacket(transform.position, transform.rotation,
                carAnimation.GetWheelSpeed, flWheelHolder.localRotation, frWheelHolder.localRotation, Client.instance.playerData))
            {
                Client.instance.SendPacket(packet.Serialize());
            }
            //Client.instance?.SendPacket(new EnemyPropertiesPacket(transform.position, transform.rotation,
            //    carAnimation.GetWheelSpeed, flWheelHolder.localRotation, frWheelHolder.localRotation).Serialize());
        }
    }
}