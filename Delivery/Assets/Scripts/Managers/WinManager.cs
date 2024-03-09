using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;  

    [SerializeField] GameObject WinPanel;
    [SerializeField] TextMeshProUGUI winnerTxt;

    #region private var
    Client thisClient;
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
        thisClient = Client.instance;
    }

    public void DeclareWinner(bool hasArrived, PlayerData playerData)
    {
        if (!hasArrived) return;
        winnerTxt.SetText($"TEAM {playerData.teamNumber} WINS!");
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DeclareWinner(bool value)
    {
        if (!value) return;
        winnerTxt.SetText($"TEAM {thisClient.playerData.teamNumber} WINS!");
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}