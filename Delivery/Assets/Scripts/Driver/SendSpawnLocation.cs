using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendSpawnLocation : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Client.instance?.SendPacket(new SpawnEnemyPacket(transform.position, transform.rotation));
        }
    }
}
