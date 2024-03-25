using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;  

    [SerializeField] GameObject WinPanel;
    [SerializeField] TextMeshProUGUI winnerTxt;
    [SerializeField] TextMeshProUGUI totalTimeTxt;

    [SerializeField] UnityEvent OnWin;

    #region private var
    ClientManager thisClient;
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
        thisClient = ClientManager.instance;
    }

    public void DeclareWinner(int teamNumber)
    {
        winnerTxt.SetText($"TEAM {teamNumber} WINS!");
        WinPanel.SetActive(true);
        TimerManager.instance?.StopTimer();
        totalTimeTxt.text = TimerManager.instance?.GetCurrentTime;
        Time.timeScale = 0f;
        OnWin.Invoke();
    }

    public void DeclareWinner(bool value)
    {
        if (!value) return;
        TimerManager.instance?.StopTimer();
        totalTimeTxt.text = TimerManager.instance?.GetCurrentTime;
        winnerTxt.SetText($"TEAM {thisClient.playerData.teamNumber} WINS!");
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
        OnWin.Invoke();
    }

    //public void DeclareWinner(bool hasArrived, PlayerData playerData)
    //{
    //    if (!hasArrived) return;
    //    winnerTxt.SetText($"TEAM {playerData.teamNumber} WINS!");
    //    WinPanel.SetActive(true);
    //    TimerManager.instance?.StopTimer();
    //    totalTimeTxt.text = TimerManager.instance?.GetCurrentTime;
    //    Time.timeScale = 0f;
    //}
}