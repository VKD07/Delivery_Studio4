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
    [SerializeField] GameObject lobbySelectionPanel, serverConnectionPanel, errorTxt;
    [SerializeField] Button closeBtn;
    [SerializeField] UnityEvent onCloseBtn;
    ClientManager thisClient;

    [Header("=== CONNECTING DOT EFFECT ===")]
    [SerializeField] TextMeshProUGUI dots;
    [SerializeField] float timeInterval = .5f;

    private void Awake()
    {
        joinServerBtn.onClick.AddListener(JoinServer);

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
        closeBtn.onClick.AddListener(CloseBtn);
        StartCoroutine(EnableConnectingDotEffect());
        thisClient = ClientManager.instance;
        DisconnectToServer();
    }

    public void JoinServer()
    {

        ClientManager.instance.ConnectToServer(ipAddressInput.text);
        serverConnectionPanel.SetActive(false);
        lobbySelectionPanel.SetActive(true);
        StartCoroutine(SendPlayerData());
    }

    IEnumerator SendPlayerData()
    {
        yield return new WaitForSeconds(.2f);
        SendPackets.SendPlayerData(thisClient.playerData.name, thisClient.playerData.teamNumber, (int)thisClient.playerData.role);
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

    void DisconnectToServer()
    {
        ClientManager.instance?.Disconnect();
    }
}