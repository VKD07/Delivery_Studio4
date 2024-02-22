using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivator : MonoBehaviour
{
    [SerializeField] GameObject[] maps;
    int mapIndex;
    private void Start()
    {
        DisableAllMaps();
        maps[mapIndex].gameObject.SetActive(true);
    }
    public void NextMap()
    {
        if(mapIndex == maps.Length-1)
        {
            mapIndex = 0;
        }
        else
        {
            mapIndex++;
        }

        DisableAllMaps();
        maps[mapIndex].gameObject.SetActive(true);
    }

    public void PrevMap()
    {
        if(mapIndex == 0)
        {
            mapIndex = maps.Length-1;
        }
        else
        {
            mapIndex--;
        }
        DisableAllMaps();
        maps[mapIndex].gameObject.SetActive(true);
    }

    void DisableAllMaps()
    {
        for(int i = 0; i < maps.Length; i++)
        {
            maps[i].gameObject.SetActive(false);
        }
    }

    #region Getters
    public GameObject GetActiveMap => maps[mapIndex];
    public GameObject[] GetMaps => maps; 
    #endregion
}
