using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(LobbyModeRequest))]
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    [SerializeField] MouseSelectionManager mouseSelectionManager;

    [Header("=== SCENES TO LOAD ===")]
    [SerializeField] string driverScene;
    [SerializeField] string navigatorScene;

    [Header("=== START GAME SETTINGS ===")]
    [SerializeField] float timeToStart = 5f;


    [Header("=== PLAYERS HANDLER ===")]
    [SerializeField] TextMeshProUGUI[] unAssignedPlayers;
    public List<string> listOfPlayerNames = new List<string>();

    [Header("=== DUO ROLES ===")]
    [SerializeField] ObjSelection driver;
    [SerializeField] ObjSelection navigator;
    [SerializeField] GameObject driveNameTxt;
    [SerializeField] GameObject navigatorNameTxt;

    [Header("=== 2V2 ROLES ===")]
    [SerializeField] ObjSelection t1_driver;
    [SerializeField] ObjSelection t1_navigator;
    [SerializeField] ObjSelection t2_driver;
    [SerializeField] ObjSelection t2_navigator;

    [SerializeField] GameObject t1_driveNameTxt;
    [SerializeField] GameObject t1_navigatorNameTxt;
    [SerializeField] GameObject t2_driverNameTxt;
    [SerializeField] GameObject t2_navigatorNameTxt;

    [Header("=== LOBBY VISUALS ===")]
    [SerializeField] GameObject backgroundNote;
    [SerializeField] GameObject unassignedPlayersPanel;
    [SerializeField] GameObject toBeDisabledNotes, searchingForPlayersPanel, selection4v4, selectionDuo;
    [SerializeField] GameObject duoNamePanel, twoVtwoNamePanel, warmUpNamePanel;
    [SerializeField] Button cancelBtn;

    [Header("=== MAIN MENU UI's ===")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject lobbySelectionPanel;

    [Header("=== CAMERAS ===")]
    [SerializeField] GameObject mainMenuCamera;
    [SerializeField] GameObject lobbyCamera, carShopCamera, navShopCamera;

    #region private vars
    ClientManager thisClient;
    GameObject btnRolePressed;
    GameObject txtChanged;
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

    private void Start()
    {
        driver.OnClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Driver, driver.gameObject, driveNameTxt, thisClient.playerData.name, true));
        navigator.OnClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Navigator, navigator.gameObject, navigatorNameTxt, thisClient.playerData.name, true));

        t1_driver.OnClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Driver, t1_driver.gameObject, t1_driveNameTxt, thisClient.playerData.name, true));
        t1_navigator.OnClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Navigator, t1_navigator.gameObject, t1_navigatorNameTxt, thisClient.playerData.name, true));

        t2_driver.OnClick.AddListener(() => SetPlayerRoleAndTeam(2, GameRole.Driver, t2_driver.gameObject, t2_driverNameTxt, thisClient.playerData.name, true));
        t2_navigator.OnClick.AddListener(() => SetPlayerRoleAndTeam(2, GameRole.Navigator, t2_navigator.gameObject, t2_navigatorNameTxt, thisClient.playerData.name, true));

        cancelBtn.onClick.AddListener(() => ChangeTeam(btnRolePressed, txtChanged, thisClient.playerData.name, true));
        cancelBtn.gameObject.SetActive(false);
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
                unAssignedPlayers[i].text = playerName;
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

    #region PLAYER ROLES
    void SetPlayerRoleAndTeam(int teamNum, GameRole role, GameObject roleBtn, GameObject playerNameTxt, string playerName, bool updatePlayerData)
    {
        //setting player data info
        if (updatePlayerData)
        {
            thisClient.playerData.teamNumber = teamNum;
            thisClient.playerData.role = role;

            //Enable Cancel Button
            cancelBtn.gameObject.SetActive(true);

            //Send this update to the network
            SendPackets.SendTeamAndRole(teamNum, (int)role, thisClient.playerData.name, (int)thisClient.playerData.mode);

            //Recording what player chosed so that we can disable it when he wants to change team
            btnRolePressed = roleBtn;
            txtChanged = playerNameTxt;
            mouseSelectionManager.enabled = false;

            Debug.Log(thisClient.playerData.teamNumber);
        }

        roleBtn.SetActive(false);
        playerNameTxt.SetActive(true);
        playerNameTxt.GetComponent<TextMeshProUGUI>().SetText(playerName);
        RemoveNameFromUnAssignedPlayerList(playerName);

        if (CheckIfAllPlayersHaveChosen())
        {
            StartCoroutine(StartGame());
        }
    }

    void ChangeTeam(GameObject btnRole, GameObject textToDisable, string playerName, bool updateData)
    {
        if (updateData)
        {
            //Disable Cancel Button
            cancelBtn.gameObject.SetActive(false);

            //Update the network that you have chosen to change team
            //NetworkSender.instance?.SendChangeTeamPacket();
            SendPackets.SendTeamChange(thisClient.playerData.teamNumber, (int)thisClient.playerData.role, thisClient.playerData.name, (int)thisClient.playerData.mode);
            mouseSelectionManager.enabled = true;
        }

        SetNameToPlayerList(playerName);
        btnRole.SetActive(true);
        textToDisable.SetActive(false);
        textToDisable.GetComponent<TextMeshProUGUI>().SetText("");

        StopAllCoroutines();
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

    #region START GAME 
    private bool CheckIfAllPlayersHaveChosen()
    {
        switch (thisClient.playerData.mode)
        {
            case LobbyMode.Duo:

                if (driver.gameObject.activeSelf || navigator.gameObject.activeSelf)
                {
                    return false;
                }
                break;
            case LobbyMode.TwoVTwo:
                if (t1_driver.gameObject.activeSelf || t2_driver.gameObject.activeSelf
                    /*|| t2_navigator.gameObject.activeSelf || t1_navigator.gameObject.activeSelf*/)
                {
                    return false;
                }
                break;
        }

        return true;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(timeToStart);

        //SendPackets.SendStartGamePacket();

        switch (thisClient.playerData.role)
        {
            case GameRole.Driver:
                SceneManager.LoadScene(driverScene);
                break;

            case GameRole.Navigator:
                SceneManager.LoadScene(navigatorScene);
                break;
        }
        //Send To network you started the game
        //NetworkSender.instance?.SendStartGamePacket();
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

        duoNamePanel.SetActive(true);

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

    public void EnableTwoVTwoLobby()
    {
        if (lobbyModeRequest.twoVTwoRequest)
        {
            lobbyModeRequest.twoVTwoRequest = false;
        }

        twoVtwoNamePanel.SetActive(true);

        mainMenuPanel.SetActive(false);
        lobbySelectionPanel.SetActive(false);

        searchingForPlayersPanel.SetActive(false);
        lobbyCamera.SetActive(true);
        mainMenuCamera.SetActive(false);
        carShopCamera.SetActive(false);
        navShopCamera.SetActive(false);

        toBeDisabledNotes.SetActive(false);
        selection4v4.SetActive(true);
        lobbyModeRequest.ResetTimer();
        EnableUnassignedPlayerPanel(1.5f);
    }

    public void DisableLobbyRequest()
    {

        lobbyModeRequest.duoRequest = false;
        lobbyModeRequest.twoVTwoRequest = false;

        lobbyModeRequest.ResetTimer();
        lobbySelectionPanel.SetActive(true);
        searchingForPlayersPanel.SetActive(false);
    }

    public void UpdateLobbyUIManager(int teamNum, GameRole role, string name, LobbyMode mode)
    {
        if (mode == LobbyMode.Duo)
        {
            if (role == GameRole.Driver)
            {
                SetPlayerRoleAndTeam(teamNum, role, driver.gameObject, driveNameTxt.gameObject, name, false);
            }
            else if (role == GameRole.Navigator)
            {
                SetPlayerRoleAndTeam(teamNum, role, navigator.gameObject, navigatorNameTxt.gameObject, name, false);

            }
        }
        else if (mode == LobbyMode.TwoVTwo)
        {
            switch (teamNum)
            {
                case 1:
                    if (role == GameRole.Driver)
                    {
                        SetPlayerRoleAndTeam(teamNum, role, t1_driver.gameObject, t1_driveNameTxt, name, false);
                    }
                    else if (role == GameRole.Navigator)
                    {
                        SetPlayerRoleAndTeam(teamNum, role, t1_navigator.gameObject, t1_navigatorNameTxt, name, false);
                    }
                    break;

                case 2:
                    if (role == GameRole.Driver)
                    {
                        SetPlayerRoleAndTeam(teamNum, role, t2_driver.gameObject, t2_driverNameTxt, name, false);
                    }
                    else if (role == GameRole.Navigator)
                    {
                        SetPlayerRoleAndTeam(teamNum, role, t2_navigator.gameObject, t2_navigatorNameTxt, name, false);
                    }
                    break;
            }
        }
    }

    public void UpdateChangedRolesFromNetwork(int teamNum, GameRole role, string playerName, LobbyMode mode)
    {
        if (mode == LobbyMode.Duo)
        {
            if (role == GameRole.Driver)
            {
                ChangeTeam(driver.gameObject, driveNameTxt, playerName, false);
            }
            else if (role == GameRole.Navigator)
            {
                ChangeTeam(navigator.gameObject, navigatorNameTxt, playerName, false);
            }
        }
        else if (mode == LobbyMode.TwoVTwo)
        {
            switch (teamNum)
            {
                case 1:
                    if (role == GameRole.Driver)
                    {
                        ChangeTeam(t1_driver.gameObject, t1_driveNameTxt, playerName, false);
                    }
                    else if (role == GameRole.Navigator)
                    {
                        ChangeTeam(t1_navigator.gameObject, t1_navigatorNameTxt, playerName, false);
                    }
                    break;

                case 2:
                    if (role == GameRole.Driver)
                    {
                        ChangeTeam(t2_driver.gameObject, t2_driverNameTxt, playerName, false);
                    }
                    else if (role == GameRole.Navigator)
                    {
                        ChangeTeam(t2_navigator.gameObject, t2_navigatorNameTxt, playerName, false);
                    }
                    break;
            }
        }
    }

    public void ReceivePacketIfGameHasStarted()
    {
        Debug.Log(thisClient.playerData.teamNumber);
        switch (thisClient.playerData.role)
        {
            case GameRole.Driver:
                SceneManager.LoadScene(driverScene);
                break;

            case GameRole.Navigator:
                SceneManager.LoadScene(navigatorScene);
                break;
        }

        //Send your data for the server to stre
    }
    #endregion
}
