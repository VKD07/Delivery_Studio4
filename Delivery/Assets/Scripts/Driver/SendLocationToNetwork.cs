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
        }

        if (hasSpawned)
        {
            Client.instance?.SendPacket(new UpdateEnemyTransformPacket(transform.position, transform.rotation));
        }
    }
}
