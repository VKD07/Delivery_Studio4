using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LobbyModeRequest))]
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    [Header("=== PLAYERS HANDLER ===")]
    [SerializeField] TextMeshProUGUI[] unAssignedPlayers;
    public List<string> listOfPlayerNames = new List<string>();

    [Header("=== LOBBY VISUALS ===")]
    [SerializeField] GameObject backgroundNote;
    [SerializeField] GameObject unassignedPlayersPanel;
    [SerializeField] GameObject toBeDisabledNotes, searchingForPlayersPanel, selection4v4, selectionDuo;

    [Header("=== MAIN MENU UI's ===")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject lobbySelectionPanel;

    [Header("=== CAMERAS ===")]
    [SerializeField] GameObject mainMenuCamera;
    [SerializeField] GameObject lobbyCamera, carShopCamera, navShopCamera;


    #region private vars
    ClientManager thisClient;
    LobbyModeRequest lobbyModeRequest => GetComponent<LobbyModeRequest>();
    #endregion

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        thisClient = FindAnyObjectByType<ClientManager>();

        toBeDisabledNotes.SetActive(true);
        selection4v4.SetActive(false);
        selectionDuo.SetActive(false);
    }

    #region UNASSIGNED PLAYERS HANDLER
    public void SetNameToPlayerList(string playerName)
    {
        //TODO : Add new name to the list, check if theres no existing name
        for (int i = 0; i < unAssignedPlayers.Length; i++)
        {
            if (!unAssignedPlayers[i].gameObject.activeSelf)
            {
                unAssignedPlayers[i].gameObject.SetActive(true);
                unAssignedPlayers[i].text += $" {playerName}";
                break;
            }
        }
    }

    public void RemoveNameFromUnAssignedPlayerList(string playerName)
    {
        for (int i = 0; i < unAssignedPlayers.Length; i++)
        {
            if (unAssignedPlayers[i].text == playerName)
            {
                unAssignedPlayers[i].gameObject.SetActive(false);
                unAssignedPlayers[i].SetText("");
                break;
            }
        }
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
            SetNameToPlayerList(playerName);
            SendJoinLobbyPacket();
        }
    }
    #endregion

    #region LOBBY UI HANDLER
    public void EnableUnassignedPlayerPanel(float delayTime)
    {
        StartCoroutine(EnableUnassignedPlayerPanelDelay(delayTime));
    }

    IEnumerator EnableUnassignedPlayerPanelDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        backgroundNote.SetActive(true);
        unassignedPlayersPanel.SetActive(true);
    }
    #endregion

    #region NETWORK SENDERS
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
    #endregion

    #region NETWORK RECEIVERS
    public void EnableDuoLobby()
    {
        if (lobbyModeRequest.duoRequest)
        {
            lobbyModeRequest.duoRequest = false;
        }

        mainMenuPanel.SetActive(false);
        lobbySelectionPanel.SetActive(false);

        searchingForPlayersPanel.SetActive(false);
        lobbyCamera.SetActive(true);
        mainMenuCamera.SetActive(false);
        carShopCamera.SetActive(false);
        navShopCamera.SetActive(false);

        toBeDisabledNotes.SetActive(false);
        selectionDuo.SetActive(true);
        lobbyModeRequest.ResetTimer();
        EnableUnassignedPlayerPanel(1.5f);
    }

    public void DisableLobbyRequest()
    {
        if (lobbyModeRequest.duoRequest)
        {
            lobbyModeRequest.duoRequest = false;
        }
        lobbyModeRequest.ResetTimer();
        lobbySelectionPanel.SetActive(true);
        searchingForPlayersPanel.SetActive(false);
    }
    #endregion
}
