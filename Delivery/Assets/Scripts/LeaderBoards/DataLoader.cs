using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataLoader : MonoBehaviour
{
    public string[] teamList;

    SortedList<TimeSpan, string> lb = new SortedList<TimeSpan, string>();

    LeaderboardUIManager leaderboardUIManager => GetComponent<LeaderboardUIManager>();
    int index;

    void Start()
    {
        StartCoroutine(GetDataFromDB());
    }

    IEnumerator GetDataFromDB()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/Delivery/PlayerTeamData.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string playerListData = www.downloadHandler.text; //geting the string data from the DB
            teamList = playerListData.Split(';'); //splitting each data with ;

            for (int i = 0; i < teamList.Length; i++)
            {
                if (!string.IsNullOrEmpty(teamList[i]))
                {
                    int index = i;

                    lb.Add(ConvertTimeSpawn(GetDataTypeOf(teamList[index], "Time:")), GetDataTypeOf(teamList[index], "Name:"));
                }
            }

            yield return null;

        }
    }

    //private void SetLeaderbordUI()
    //{
    //    //Set leader boards
    //    foreach (var kvp in lb)
    //    {
    //        leaderboardUIManager.SetLeaderBoardUI(index, kvp.Value, kvp.Key.ToString());
    //        index++;
    //    }
    //}

    string GetDataTypeOf(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);

        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }

    public TimeSpan ConvertTimeSpawn(string time)
    {
        string[] spitTime = time.Split(':');
        return new TimeSpan(int.Parse(spitTime[0]), int.Parse(spitTime[1]), int.Parse(spitTime[2]));
    }
}
