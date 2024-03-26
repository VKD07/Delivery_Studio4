using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LobbyUIManager))]
public class PlayerLobbyManager : MonoBehaviour
{
    public static PlayerLobbyManager instance;
    public List<string> listOfPlayerNames = new List<string>();
    public LobbyUIManager lobbyUIManager => GetComponent<LobbyUIManager>();
    ClientManager thisClient;
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

    }

    private void Start()
    {
        thisClient = ClientManager.instance;
    }
    public void SendJoinLobbyPacket()
    {
        AddPlayerToTheList(thisClient.playerData.name);
        SendPackets.SendJoinLobbyPacket(thisClient.playerData.name);
        //NetworkSender.instance?.SendLobbyJoinPacket();
    }

    public IEnumerator SendJoinLobbyPacket(float time)
    {
        yield return new WaitForSeconds(time);
        AddPlayerToTheList(thisClient.playerData.name);
        SendPackets.SendJoinLobbyPacket(thisClient.playerData.name);
        //NetworkSender.instance?.SendLobbyJoinPacket();
    }

    public void UpdatePlayerListAndSendNameToNetwork(string name)
    {
        AddPlayerToTheList(name);
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
