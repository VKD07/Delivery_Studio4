using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance { get; private set; }

    [SerializeField] TextMeshProUGUI[] unAssignedPlayers;
    [SerializeField] GameObject notEnoughPlayersTxt;
    [SerializeField] GameObject[] teamPanels;
    [SerializeField] Button startGameBtn;

    [Header("=== SCENE TO LOAD ===")]
    [SerializeField] string driverScene;
    [SerializeField] string navigatorScene;

    [Header("=== TEAM 1 PANEL ===")]
    [SerializeField] GameObject t1_driverPanel;
    private Button t1_chooseDriverBtn;
    private TextMeshProUGUI t1_driverName;

    [SerializeField] GameObject t1_navigatorPanel;
    private Button t1_chosenNavigatorBtn;
    private TextMeshProUGUI t1_navigatorName;

    [Header("=== TEAM 2 PANEL ===")]
    [SerializeField] GameObject t2_driverPanel;
    private Button t2_chooseDriverBtn;
    private TextMeshProUGUI t2_driverName;

    [SerializeField] GameObject t2_navigatorPanel;
    private Button t2_chosenNavigatorBtn;
    private TextMeshProUGUI t2_navigatorName;

    [Header("=== CANCEL BUTTON ===")]
    [SerializeField] Button cancelButton;

    public PlayerLobbyManager playerLobbyManager => GetComponent<PlayerLobbyManager>();

    #region private vars
    Client thisClient;
    GameObject btnRolePressed;
    GameObject txtChanged;
    #endregion

    #region Setters
    public void SetNameToPlayerList(string playerName)
    {
        //TODO : Add new name to the list, check if theres no existing name
        for (int i = 0; i < unAssignedPlayers.Length; i++)
        {
            if (!unAssignedPlayers[i].gameObject.activeSelf)
            {
                unAssignedPlayers[i].gameObject.SetActive(true);
                unAssignedPlayers[i].SetText(playerName);
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

        thisClient = FindObjectOfType<Client>();

        //Team 1 Driver Button
        t1_chooseDriverBtn = t1_driverPanel.GetComponentInChildren<Button>();
        t1_driverName = t1_driverPanel.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        //Team 1 Navigator Button
        t1_chosenNavigatorBtn = t1_navigatorPanel.GetComponentInChildren<Button>();
        t1_navigatorName = t1_navigatorPanel.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();

        //Team 2 Driver Button
        t2_chooseDriverBtn = t2_driverPanel.GetComponentInChildren<Button>();
        t2_driverName = t2_driverPanel.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        //Team 2 Navigator Button
        t2_chosenNavigatorBtn = t2_navigatorPanel.GetComponentInChildren<Button>();
        t2_navigatorName = t2_navigatorPanel.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();

        //Disable Cancel Btn
        cancelButton.gameObject.SetActive(false);
    }

    private void Start()
    {

        t1_chooseDriverBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Driver, t1_chooseDriverBtn.gameObject,
                                                                       t1_driverName.gameObject, thisClient.playerData.name, true));
        t1_chosenNavigatorBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Navigator, t1_chosenNavigatorBtn.gameObject,
                                                                               t1_navigatorName.gameObject, thisClient.playerData.name, true));

        t2_chooseDriverBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(2, GameRole.Driver, t2_chooseDriverBtn.gameObject,
                                                                       t2_driverName.gameObject, thisClient.playerData.name, true));
        t2_chosenNavigatorBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(2, GameRole.Navigator, t2_chosenNavigatorBtn.gameObject,
                                                                               t2_navigatorName.gameObject, thisClient.playerData.name, true));

        cancelButton.onClick.AddListener(() => ChangeTeam(btnRolePressed, txtChanged, thisClient.playerData, true));

        startGameBtn.onClick.AddListener(StartGame);

        if (thisClient == null) { Debug.Log("NoClient"); }
    }

    private void Update()
    {
        EnableGameReady();
        CheckAndEnableRoleBtns();
    }

    private void EnableGameReady()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            startGameBtn.gameObject.SetActive(true);
        }
    }

    void SetTeamPanels(bool value)
    {
        for (int i = 0; i < teamPanels.Length; i++)
        {
            teamPanels[i].SetActive(value);
        }

        notEnoughPlayersTxt.SetActive(!value);
    }

    #region Network Senders

    void CheckAndEnableRoleBtns()
    {
        if (playerLobbyManager.listOfPlayerNames.Count >= 4)
        {
            SetTeamPanels(true);
        }
        else
        {
            SetTeamPanels(false);
        }
    }

    void SetPlayerRoleAndTeam(int teamNum, GameRole role, GameObject roleBtn, GameObject playerNameTxt, string playerName, bool updatePlayerData)
    {
        //setting player data info
        if (updatePlayerData)
        {
            thisClient.playerData.teamNumber = teamNum;
            thisClient.playerData.role = role;

            //Enable Cancel Button
            cancelButton.gameObject.SetActive(true);

            //Send this update to the network
            NetworkSender.instance?.SendRoleAndTeamPacket();

            //Recording what player chosed so that we can disable it when he wants to change team
            btnRolePressed = roleBtn;
            txtChanged = playerNameTxt;
        }

        roleBtn.SetActive(false);
        playerNameTxt.SetActive(true);
        playerNameTxt.GetComponent<TextMeshProUGUI>().SetText(playerName);
        RemoveNameFromUnAssignedPlayerList(playerName);
    }

    void ChangeTeam(GameObject btnRole, GameObject textToDisable, PlayerData playerData, bool updateData)
    {
        if (updateData)
        {
            //Disable Cancel Button
            cancelButton.gameObject.SetActive(false);

            //Update the network that you have chosen to change team
            NetworkSender.instance?.SendChangeTeamPacket();
        }

        SetNameToPlayerList(playerData.name);
        btnRole.SetActive(true);
        textToDisable.SetActive(false);
        textToDisable.GetComponent<TextMeshProUGUI>().SetText("");
    }

    void StartGame()
    {
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
        NetworkSender.instance?.SendStartGamePacket();
    }
    #endregion

    #region Network Receivers

    public void UpdateLobbyUIManager(PlayerData playerData)
    {
        switch (playerData.teamNumber)
        {
            case 1:
                if (playerData.role == GameRole.Driver)
                {
                    SetPlayerRoleAndTeam(playerData.teamNumber, playerData.role, t1_chooseDriverBtn.gameObject, t1_driverName.gameObject, playerData.name, false);
                }
                else if (playerData.role == GameRole.Navigator)
                {
                    SetPlayerRoleAndTeam(playerData.teamNumber, playerData.role, t1_chosenNavigatorBtn.gameObject, t1_navigatorName.gameObject, playerData.name, false);
                }
                break;

            case 2:
                if (playerData.role == GameRole.Driver)
                {
                    SetPlayerRoleAndTeam(playerData.teamNumber, playerData.role, t2_chooseDriverBtn.gameObject, t2_driverName.gameObject, playerData.name, false);
                }
                else if (playerData.role == GameRole.Navigator)
                {
                    SetPlayerRoleAndTeam(playerData.teamNumber, playerData.role, t2_chosenNavigatorBtn.gameObject, t2_navigatorName.gameObject, playerData.name, false);
                }
                break;
        }
    }

    public void UpdateChangedRolesFromNetwork(PlayerData playerData)
    {
        switch (playerData.teamNumber)
        {
            case 1:
                if (playerData.role == GameRole.Driver)
                {
                    ChangeTeam(t1_chooseDriverBtn.gameObject, t1_driverName.gameObject, playerData, false);
                }
                else if (playerData.role == GameRole.Navigator)
                {
                    ChangeTeam(t1_chosenNavigatorBtn.gameObject, t1_navigatorName.gameObject, playerData, false);
                }
                break;

            case 2:
                if (playerData.role == GameRole.Driver)
                {
                    ChangeTeam(t2_chooseDriverBtn.gameObject, t2_driverName.gameObject, playerData, false);
                }
                else if (playerData.role == GameRole.Navigator)
                {
                    ChangeTeam(t2_chosenNavigatorBtn.gameObject, t2_navigatorName.gameObject, playerData, false);
                }
                break;
        }
    }

    public void ReceivePacketIfGameHasStarted(bool hasStarted)
    {
        if (!hasStarted) { return; }

        switch (thisClient.playerData.role)
        {
            case GameRole.Driver:
                SceneManager.LoadScene(driverScene);
                break;

            case GameRole.Navigator:
                SceneManager.LoadScene(navigatorScene);
                break;
        }
    }
    #endregion
}
