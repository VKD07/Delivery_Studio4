using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivator : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnedMaps;
    [SerializeField] Transform mapsParent;
    [SerializeField] List<TargetLocation> targets;
    int mapIndex;

    private void Awake()
    {
        InitAllMap();
    }
    private void OnEnable()
    {
    }

    private void Start()
    {
        DisableAllMaps();
        spawnedMaps[mapIndex].gameObject.SetActive(true);
    }

    void InitAllMap()
    {
        try
        {
            GameObject[] maps = MapChooser.instance.mapsChosen;
            for (int i = 0; i < maps.Length; i++)
            {
                GameObject spawnedMap = Instantiate(maps[i], mapsParent.position, Quaternion.identity);
                spawnedMap.transform.parent = mapsParent;
                spawnedMaps.Add(spawnedMap);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Map Choose Not Detected!");
        }
    }

    public void NextMap()
    {
        if (mapIndex == spawnedMaps.Count - 1)
        {
            mapIndex = 0;
        }
        else
        {
            mapIndex++;
        }

        DisableAllMaps();
        spawnedMaps[mapIndex].gameObject.SetActive(true);
    }

    public void PrevMap()
    {
        if (mapIndex == 0)
        {
            mapIndex = spawnedMaps.Count - 1;
        }
        else
        {
            mapIndex--;
        }
        DisableAllMaps();
        spawnedMaps[mapIndex].gameObject.SetActive(true);
    }

    void DisableAllMaps()
    {
        for (int i = 0; i < spawnedMaps.Count; i++)
        {
            spawnedMaps[i].gameObject.SetActive(false);
        }
    }

    #region Getters
    public GameObject GetActiveMap => spawnedMaps[mapIndex];
    //public GameObject[] GetMaps => maps; 
    #endregion
}
