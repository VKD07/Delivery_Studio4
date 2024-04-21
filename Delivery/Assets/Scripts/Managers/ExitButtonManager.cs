using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonManager : MonoBehaviour
{
    SceneLoaderManager sceneLoader;

    private void Start()
    {
        sceneLoader = SceneLoaderManager.instance;
    }

    public void ExitToMenu()
    {
        ResetPlayerData();
        sceneLoader.LoadNextScene(sceneLoader.mainMenuScene);
    }

    void ResetPlayerData()
    {
        ClientManager.instance.playerData.winner = 0;
        ClientManager.instance.playerData.time = "";
        ClientManager.instance.playerData.mode = LobbyMode.None;
        ClientManager.instance.playerData.role = GameRole.None;
        ClientManager.instance.playerData.teamNumber = 0;
        HandlePackets.newRatingData = false;
    }
}
