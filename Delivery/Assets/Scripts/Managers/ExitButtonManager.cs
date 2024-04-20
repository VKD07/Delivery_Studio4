using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonManager : MonoBehaviour
{
    [SerializeField] string mainMenuScene = "MainMenu";
    public void ExitToMenu()
    {
        ResetPlayerData();
        SceneManager.LoadScene(mainMenuScene);
    }

    void ResetPlayerData()
    {
        ClientManager.instance.playerData.winner = 0;
        ClientManager.instance.playerData.time = "";
        ClientManager.instance.playerData.mode = LobbyMode.None;
        ClientManager.instance.playerData.role = GameRole.None;
        ClientManager.instance.playerData.teamNumber = 0;
    }
}
