using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ServerConnection : MonoBehaviour
{
    public static ServerConnection instance;

    [SerializeField] TMP_InputField ipAddressInput;
    [SerializeField] Button joinServerBtn;
    [SerializeField] GameObject lobbySelectionPanel, serverConnectionPanel, connectionFailurePanel;
    [SerializeField] Button closeBtn;
    [SerializeField] UnityEvent onCloseBtn;
    ClientManager thisClient;

    [Header("=== CONNECTING DOT EFFECT ===")]
    [SerializeField] TextMeshProUGUI dots;
    [SerializeField] float timeInterval = .5f;
    bool connectedSuccessfully;
    bool isConnecting;

    UserDataManager userDataManager;

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

        joinServerBtn.onClick.AddListener(JoinServer);
    }

    private void Start()
    {
        thisClient = ClientManager.instance;
        userDataManager = UserDataManager.instance;

        closeBtn.onClick.AddListener(CloseBtn);
        SetPreviousIPAddress();
        DisconnectToServer();
    }

    public void JoinServer()
    {
        if (!isConnecting)
        {
            isConnecting = true;
            
            ClientManager.instance.ConnectToServer(ipAddressInput.text);
            SaveNewIP();
            StopAllCoroutines();
            StartCoroutine(EnableConnectingDotEffect());
            StartCoroutine(ServerConnectionDelay());
        }
    }

    public void SetPreviousIPAddress()
    {
        if (userDataManager.CheckPrevIPAddress())
        {
            ipAddressInput.text = userDataManager.GetPreviousIP();
        }
    }

    void SaveNewIP()
    {
        userDataManager.SaveIPAddress(ipAddressInput.text);
    }

    IEnumerator ServerConnectionDelay()
    {
        yield return new WaitForSeconds(3.2f);
        if (connectedSuccessfully)
        {
            EnableLobbyPanel();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ErrorConnection());
        }
        isConnecting = false;
    }

    IEnumerator EnableConnectingDotEffect()
    {
        while (true)
        {
            yield return null;
            dots.text = "";
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(timeInterval);
                dots.text += ".";
            }
        }
    }

    void CloseBtn()
    {
        onCloseBtn.Invoke();
        serverConnectionPanel.SetActive(false);
    }

    void EnableLobbyPanel()
    {
        serverConnectionPanel.SetActive(false);
        lobbySelectionPanel.SetActive(true);
        StartCoroutine(SendPlayerData());
    }

    IEnumerator SendPlayerData()
    {
        yield return new WaitForSeconds(.2f);
        SendPackets.SendPlayerData(thisClient.playerData.name, thisClient.playerData.teamNumber, (int)thisClient.playerData.role);
    }

    IEnumerator ErrorConnection()
    {
        connectionFailurePanel.SetActive(true);
        yield return new WaitForSeconds(2);
        connectionFailurePanel.SetActive(false);
        dots.text = "......";
    }

    public void DisconnectToServer()
    {
        ClientManager.instance?.Disconnect();
    }

    #region NETWORK RECEIVERS
    public void ServerConnectionSuccess()
    {
        connectedSuccessfully = true;
    }
    #endregion
}