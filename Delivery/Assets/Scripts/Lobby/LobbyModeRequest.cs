using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class LobbyModeRequest : MonoBehaviour
{
    [SerializeField] GameObject searchingForPlayersPanel;
    [SerializeField] float requestTimeLimit = 25f;
    [HideInInspector] public bool duoRequest;
    [HideInInspector] public bool twoVTwoRequest;
    public float currentTime;

    [Header("=== DOT EFFECT ===")]
    [SerializeField] TextMeshProUGUI dotsEffect;
    [SerializeField] float timeInterval = .5f;

    LobbyManager lobbyManager => GetComponent<LobbyManager>();

    private void Start()
    {
        StartCoroutine(EnableConnectingDotEffect());
    }

    private void Update()
    {
        Timer();

        if (duoRequest)
        {
            RequestDuoLobbyMode();
        }
        else if (twoVTwoRequest)
        {
            Request2V2LobbyMode();
        }
    }

    public void RequestDuoLobbyMode()
    {
        duoRequest = true;
        ClientManager.instance.playerData.mode = LobbyMode.Duo;
        SendPackets.SendLobbyModeRequest(ClientManager.instance.playerData.mode);
    }

    public void Request2V2LobbyMode()
    {
        twoVTwoRequest = true;
        ClientManager.instance.playerData.mode = LobbyMode.TwoVTwo;
        SendPackets.SendLobbyModeRequest(ClientManager.instance.playerData.mode);
    }

    IEnumerator EnableConnectingDotEffect()
    {
        while (true)
        {
            yield return null;
            dotsEffect.text = "";
            for (int i = 0; i < 7; i++)
            {
                yield return new WaitForSeconds(timeInterval);
                dotsEffect.text += ".";
            }
        }
    }

    void Timer()
    {
        if (!duoRequest && !twoVTwoRequest) return;

        if (currentTime < requestTimeLimit)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            duoRequest = false;
            twoVTwoRequest = false;
            lobbyManager.DisableLobbyRequest();
            searchingForPlayersPanel.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        currentTime = 0;
    }
}
