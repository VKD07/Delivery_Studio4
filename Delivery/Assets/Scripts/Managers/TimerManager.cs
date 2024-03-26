using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [SerializeField] TextMeshProUGUI currentTimeTxt;
    bool timerActive = true;
    float currentTime;
    int startMinutes;

    #region Getter
    public string GetCurrentTime => currentTimeTxt.text;
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
    }

    void Start()
    {
        currentTime = 0;
    }

    void Update()
    {
        TimerCounting();
    }

    #region Network Sender
    private void TimerCounting()
    {
        if (timerActive && ClientManager.instance?.playerData.role == GameRole.Navigator)
        {
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeTxt.text = time.ToString(@"hh\:mm\:ss");

            //NetworkSender.instance?.SendCurrentTimer(currentTimeTxt.text);
            SendPackets.SendTimer(currentTimeTxt.text);
        }
    }

    #endregion

    #region Network Receiver
    public void UpdateTimer(string timer)
    {
        currentTimeTxt.text = timer;
    }
    //public void UpdateTimer(string timer, PlayerData playerData)
    //{
    //    if (playerData.teamNumber != Client.instance?.playerData.teamNumber) return;
    //    currentTimeTxt.text = timer;
    //}
    #endregion

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }
}