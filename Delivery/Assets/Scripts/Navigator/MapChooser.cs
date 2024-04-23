using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChooser : MonoBehaviour
{
    public static MapChooser instance;

    public NavigatorMapCreator[] setOfMaps;
    public int chosenSetOfMapID;
    ClientManager clientManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        clientManager = ClientManager.instance;
    }

    public void ChooseARandomSetOfMapAndLoad()
    {
        if (clientManager.playerData.role == GameRole.Navigator)
        {
            chosenSetOfMapID = Random.Range(0, setOfMaps.Length);
            SendPackets.SendChosenMapID(chosenSetOfMapID);
            SceneLoaderManager.instance.LoadNextScene(SceneLoaderManager.instance.navigatorScene);
        }
    }

    #region NetworkReceivers
    public void LoadDriverToChosenMap(int chosenMapID)
    {
        SceneLoaderManager.instance?.LoadNextScene(setOfMaps[chosenMapID].sceneName);
    }
    #endregion

    public GameObject[] mapsChosen => setOfMaps[chosenSetOfMapID].maps;
}
