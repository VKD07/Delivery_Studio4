using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerConnection : MonoBehaviour
{
    public static ServerConnection instance;

    [SerializeField] TMP_InputField ipAddressInput;
    [SerializeField] Button joinServerBtn;
    [SerializeField] GameObject lobbySelectionPanel, serverConnectionPanel, errorTxt;
    ClientManager thisClient;
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
        thisClient = ClientManager.instance;
    }

    public void JoinServer()
    {
        try
        {
            ClientManager.instance.ConnectToServer(ipAddressInput.text);
            serverConnectionPanel.SetActive(false);
            lobbySelectionPanel.SetActive(true);

            StartCoroutine(SendPlayerData());

        }
        catch (System.Exception)
        {
            errorTxt.SetActive(true);
            Debug.Log("Failed to connect to server");
        }
    }

    IEnumerator SendPlayerData()
    {
        yield return new WaitForSeconds(.2f);
        SendPackets.SendPlayerData(thisClient.playerData.name, thisClient.playerData.teamNumber, (int)thisClient.playerData.role);
    }
}