using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendLocationToNetwork : MonoBehaviour
{
    bool hasSpawned;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !hasSpawned)
        {
            hasSpawned = true;
            Client.instance?.SendPacket(new SpawnEnemyPacket(transform.position, transform.rotation));
            Debug.Log("Has spawned enemy");
        }

        if (hasSpawned)
        {
            Client.instance?.SendPacket(new UpdateEnemyTransformPacket(transform.position, transform.rotation));
        }
    }
}
