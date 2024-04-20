using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataInserter : MonoBehaviour
{
    public static DataInserter instance;
    bool partnerHasSent;
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

    string phpURL = "http://localhost/Delivery/InsertLeaderboard.php";

    public void InsertNewTeamAndRecord(string teamName, string time)
    {
        StartCoroutine(InsertTeamAndRecordToDB(teamName, time));
    }
    IEnumerator InsertTeamAndRecordToDB(string teamName, string time)
    {
        WWWForm form = new WWWForm();
        form.AddField("teamName", teamName);
        form.AddField("totalTime", time);

        using (UnityWebRequest www = UnityWebRequest.Post(phpURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST request failed: " + www.error);
            }
            else
            {
                Debug.Log("POST request successful!");
            }
        }
    }
}