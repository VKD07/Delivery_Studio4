using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

[RequireComponent(typeof(LobbyUIManager))]
public class PlayerLobbyManager : MonoBehaviour
{
    public static PlayerLobbyManager instance;
    public List<string> listOfPlayerNames = new List<string>();
    public LobbyUIManager lobbyUIManager => GetComponent<LobbyUIManager>();
    public Client client;
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

    private void OnEnable()
    {
        client.onLobbyJoined += UpdatePlayerListAndSendNameToNetwork;
    }

    private void OnDisable()
    {
        client.onLobbyJoined -= UpdatePlayerListAndSendNameToNetwork;
    }

    public void SendJoinLobbyPacket()
    {
        AddPlayerToTheList(client.playerData.name);
        using (JoinServerPacket packet = new JoinServerPacket(client.playerData))
        {
            client.SendPacket(packet.Serialize());
        }
    }

    public void UpdatePlayerListAndSendNameToNetwork(JoinServerPacket packet)
    {
        AddPlayerToTheList(packet.playerData.name);
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
