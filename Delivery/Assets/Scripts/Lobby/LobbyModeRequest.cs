using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class LobbyModeRequest : MonoBehaviour
{
    [SerializeField] float requestTimeLimit = 25f;
    [HideInInspector] public bool duoRequest;
    public float currentTime;

    LobbyManager lobbyManager => GetComponent<LobbyManager>();

    private void Update()
    {
        Timer();

        if (duoRequest)
        {
            RequestDuoLobbyMode();
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
        ClientManager.instance.playerData.mode = LobbyMode.TwoVTwo;
        SendPackets.SendLobbyModeRequest(ClientManager.instance.playerData.mode);
    }

    void Timer()
    {
        if (!duoRequest) return;

        if(currentTime < requestTimeLimit)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            duoRequest = false;
            lobbyManager.DisableLobbyRequest();
        }
    }

    public void ResetTimer()
    {
        currentTime = 0;
    }
}
