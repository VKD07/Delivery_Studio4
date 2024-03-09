using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LobbyUIManager))]
public class PlayerLobbyManager : MonoBehaviour
{
    public static PlayerLobbyManager instance;
    public List<string> listOfPlayerNames = new List<string>();
    public LobbyUIManager lobbyUIManager => GetComponent<LobbyUIManager>();
    Client thisClient;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        thisClient = FindFirstObjectByType<Client>();
    }
    public void SendJoinLobbyPacket()
    {
        AddPlayerToTheList(thisClient.playerData.name);
        NetworkSender.instance?.SendLobbyJoinPacket();
    }

    public void UpdatePlayerListAndSendNameToNetwork(PlayerData playerData)
    {
        AddPlayerToTheList(playerData.name);
    }

    void AddPlayerToTheList(string playerName)
    {
        if (!listOfPlayerNames.Contains(playerName))
        {
            listOfPlayerNames.Add(playerName);
            lobbyUIManager.SetNameToPlayerList(playerName);
            SendJoinLobbyPacket();
        }
    }
}
