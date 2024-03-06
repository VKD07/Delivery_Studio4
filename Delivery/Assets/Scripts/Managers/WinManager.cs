using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;  

    [SerializeField] GameObject WinPanel;

    #region private var
    Client client;
    #endregion
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
        client = Client.instance;
    }
    private void OnEnable()
    {
        if (client == null) return;
        client.onDriverArrived += DeclareWinner;
    }

    private void OnDisable()
    {
        if (client == null) return;
        client.onDriverArrived -= DeclareWinner;
    }

    public void DeclareWinner(DriverArrivedPacket packet)
    {
        if (!packet.hasArrived) return;
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DeclareWinner(bool value)
    {
        if (!value) return;
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}