using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class MainMenuEnvironmentManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float moveSpeed = .5f;

    [SerializeField] Transform[] roads;
    [SerializeField] float wheelSpeed;
    [SerializeField] Transform[] wheels;
    [SerializeField] public Vector3 nextSpawnPoint = Vector3.zero;
    [SerializeField] Vector3 current;
    [SerializeField] Vector3 firstSpawn;


    public LinkedList<Transform> roadList = new LinkedList<Transform>();

    private void Update()
    {
        RotateCarWheels();
    }

    private void Awake()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            GameObject temp = Instantiate(roads[i].gameObject, firstSpawn, Quaternion.identity);
            firstSpawn = temp.transform.Find("FirstSpawnPoint").position;
            current = temp.transform.position;
            temp.GetComponent<RoadTile>().environmentManager = this;
            roadList.AddLast(temp.transform);
        }
    }


    void RotateCarWheels()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Rotate(wheelSpeed, 0, 0);
        }
    }
}
