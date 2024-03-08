using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;

    [SerializeField] TextMeshProUGUI[] unAssignedPlayers;

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

    public Client client;
    #region private vars
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

    private void OnEnable()
    {
        client.onPlayerTeamAndRole += UpdateLobbyUIManager;
        client.onChangeTeam += UpdateChangedRolesFromNetwork;
    }
    private void OnDisable()
    {
        client.onPlayerTeamAndRole -= UpdateLobbyUIManager;
        client.onChangeTeam -= UpdateChangedRolesFromNetwork;
    }


    private void Start()
    {
        t1_chooseDriverBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Driver, t1_chooseDriverBtn.gameObject,
                                                                       t1_driverName.gameObject, client.playerData.name, true));
        t1_chosenNavigatorBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(1, GameRole.Navigator, t1_chosenNavigatorBtn.gameObject,
                                                                               t1_navigatorName.gameObject, client.playerData.name, true));

        t2_chooseDriverBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(2, GameRole.Driver, t2_chooseDriverBtn.gameObject,
                                                                       t2_driverName.gameObject, client.playerData.name, true));
        t2_chosenNavigatorBtn.onClick.AddListener(() => SetPlayerRoleAndTeam(2, GameRole.Navigator, t2_chosenNavigatorBtn.gameObject,
                                                                               t2_navigatorName.gameObject, client.playerData.name, true));

        cancelButton.onClick.AddListener(() => ChangeTeam(btnRolePressed, txtChanged, client.playerData, true));

        if (client == null) { Debug.Log("NoClient"); }
    }

    void SetPlayerRoleAndTeam(int teamNum, GameRole role, GameObject roleBtn, GameObject playerNameTxt, string playerName, bool updatePlayerData)
    {
        //setting player data info
        if (updatePlayerData)
        {
            client.playerData.teamNumber = teamNum;
            client.playerData.role = role;

            //Enable Cancel Button
            cancelButton.gameObject.SetActive(true);

            //Send this update to the network
            using (TeamAndRolePacket packet = new TeamAndRolePacket(client.playerData))
            {
                client?.SendPacket(packet.Serialize());
            }

            //Recording what player chosed so that we can disable it when he wants to change team
            btnRolePressed = roleBtn;
            txtChanged = playerNameTxt;
        }

        roleBtn.SetActive(false);
        playerNameTxt.SetActive(true);
        playerNameTxt.GetComponent<TextMeshProUGUI>().SetText(playerName);
        RemoveNameFromUnAssignedPlayerList(playerName);
    }
    //TODO: Fix Sometimes text isnt Disabling
    void ChangeTeam(GameObject btnRole, GameObject textToDisable, PlayerData playerData, bool updateData)
    {
        if (updateData)
        {
            //Disable Cancel Button
            cancelButton.gameObject.SetActive(false);
            //Update the network that you have chosen to change team
            using (ChangeTeamPacket packet = new ChangeTeamPacket(client.playerData))
            {
                client?.SendPacket(packet.Serialize());
            }
        }

        SetNameToPlayerList(playerData.name);
        btnRole.SetActive(true);
        textToDisable.SetActive(false);
        textToDisable.GetComponent<TextMeshProUGUI>().SetText("");
    }


    void NetworkPlayerRoleAndTeam(int teamNum, GameRole role, GameObject roleBtn, GameObject playerNameTxt, string playerName, bool updatePlayerData)
    {
        //setting player data info
        if (updatePlayerData)
        {
            client.playerData.teamNumber = teamNum;
            client.playerData.role = role;

            //Enable Cancel Button
            cancelButton.gameObject.SetActive(true);

            //Send this update to the network
            using (TeamAndRolePacket packet = new TeamAndRolePacket(client.playerData))
            {
                client?.SendPacket(packet.Serialize());
            }
        }

        roleBtn.SetActive(false);
        playerNameTxt.SetActive(true);
        playerNameTxt.GetComponent<TextMeshProUGUI>().SetText(playerName);
        RemoveNameFromUnAssignedPlayerList(playerName);

        //Recording what player chosed so that we can disable it when he wants to change team
        btnRolePressed = roleBtn;
        txtChanged = playerNameTxt;
    }

    #region Network Receivers
    void UpdateLobbyUIManager(TeamAndRolePacket packet)
    {
        switch (packet.playerData.teamNumber)
        {
            case 1:
                if (packet.playerData.role == GameRole.Driver)
                {
                    SetPlayerRoleAndTeam(packet.playerData.teamNumber, packet.playerData.role, t1_chooseDriverBtn.gameObject, t1_driverName.gameObject, packet.playerData.name, false);
                }
                else if (packet.playerData.role == GameRole.Navigator)
                {
                    SetPlayerRoleAndTeam(packet.playerData.teamNumber, packet.playerData.role, t1_chosenNavigatorBtn.gameObject, t1_navigatorName.gameObject, packet.playerData.name, false);
                }
                break;

            case 2:
                if (packet.playerData.role == GameRole.Driver)
                {
                    SetPlayerRoleAndTeam(packet.playerData.teamNumber, packet.playerData.role, t2_chooseDriverBtn.gameObject, t2_driverName.gameObject, packet.playerData.name, false);
                }
                else if(packet.playerData.role == GameRole.Navigator)
                {
                    SetPlayerRoleAndTeam(packet.playerData.teamNumber, packet.playerData.role, t2_chosenNavigatorBtn.gameObject, t2_navigatorName.gameObject, packet.playerData.name, false);
                }
                break;
        }
    }

    void UpdateChangedRolesFromNetwork(ChangeTeamPacket packet)
    {
        switch (packet.playerData.teamNumber)
        {
            case 1:
                if (packet.playerData.role == GameRole.Driver)
                {
                    ChangeTeam(t1_chooseDriverBtn.gameObject, t1_driverName.gameObject, packet.playerData, false);
                }
                else if (packet.playerData.role == GameRole.Navigator)
                {
                    ChangeTeam(t1_chosenNavigatorBtn.gameObject, t1_navigatorName.gameObject, packet.playerData, false);
                }
                break;

            case 2:
                if (packet.playerData.role == GameRole.Driver)
                {
                    ChangeTeam(t2_chooseDriverBtn.gameObject, t2_driverName.gameObject, packet.playerData, false);
                }
                else if (packet.playerData.role == GameRole.Navigator)
                {
                    ChangeTeam(t2_chosenNavigatorBtn.gameObject, t2_navigatorName.gameObject, packet.playerData, false);
                }
                break;
        }
    }
    #endregion
}
