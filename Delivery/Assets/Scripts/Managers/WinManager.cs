using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;  

    [SerializeField] GameObject WinPanel;

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
    }

    private void OnEnable()
    {
        Client.instance.onDriverArrived += DeclareWinner;
    }

    private void OnDisable()
    {
        Client.instance.onDriverArrived -= DeclareWinner;
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
