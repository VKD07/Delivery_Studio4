using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardUIManager : MonoBehaviour
{
    public static LeaderboardUIManager instance;
    [NonReorderable]
    public TeamRecord[] leaderBoardUI;
    public bool partnerHasSent;

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

    public void SetLeaderBoardUI(int index, string teamName, string time)
    {
        if (index > leaderBoardUI.Length - 1) return;
        leaderBoardUI[index].teamName.text = teamName;
        leaderBoardUI[index].time.text = time;
    }

    #region Network Senders
    public void SendTeamDataToDB()
    {
        if (partnerHasSent)
        {
            StopCoroutine(SendDataWithDelay());
            return;
        }
        StartCoroutine(SendDataWithDelay());
    }

    IEnumerator SendDataWithDelay()
    {
        //Making sure both player doesnt send twice
        int randomTime = Random.Range(0, 3);
        yield return new WaitForSeconds(randomTime);
        SendPackets.SendTeamRecord(ClientManager.instance?.playerData.name, ClientManager.instance?.playerData.time);
    }
    #endregion
}

[System.Serializable]
public class TeamRecord
{
    public TextMeshProUGUI teamName;
    public TextMeshProUGUI time;
}
