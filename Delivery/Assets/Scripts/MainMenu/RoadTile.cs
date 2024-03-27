using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : MonoBehaviour
{
   public MainMenuEnvironmentManager environmentManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            environmentManager.roadList.Last.Value.position = transform.Find("NextSpawnPoint").position;
            environmentManager.roadList.AddFirst(environmentManager.roadList.Last.Value);
            environmentManager.roadList.RemoveLast();
        }
    }
}
