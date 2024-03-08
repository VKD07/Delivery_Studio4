using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Temp_UIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TMP_InputField ipAddressInput;
    [SerializeField] Button joinServerBtn;
    [SerializeField] GameObject logInPanel, lobbyPanel;
    [SerializeField] PlayerLobbyManager playerLobbyManager;

    private void Awake()
    {
        logInPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        joinServerBtn.onClick.AddListener(JoinServer);
    }

    public void JoinServer()
    {
        try
        {
            Client.instance.ConnectToServer(ipAddressInput.text, playerName.text, 0, GameRole.None);
            playerLobbyManager.SendJoinLobbyPacket();
            logInPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
        catch (System.Exception)
        {
            Debug.Log("Failed to connect to server");
        }
    
    }
}
