using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogleForm : MonoBehaviour
{    
    //The link of the google form response
    private string BASE_URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLSf9aNBbPNOhHDXxQ35scuK_UuXQQhrok_FiU8nrIgcpcOHHfw/formResponse";

    CollectData collectData;
    private void Awake()
    {
        collectData = CollectData.instance;
    }

    //Add data inside the parenthesis
    IEnumerator Post(string crashCount, string playTime, string playerCount)
    {
        WWWForm form = new WWWForm();

        //All the input responses here
        //add inputfield ID here.
        form.AddField("entry.739522593", crashCount);
        form.AddField("entry.1245199307", playTime);
        form.AddField("entry.1345776529", playerCount);

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL, form);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }


    //Sends the data to google form
    public void Send()
    {
        StartCoroutine(Post(collectData.crashCount.ToString(), collectData.averagePlayTime.ToString(), collectData.playerCount.ToString()));
    }

    private void OnApplicationQuit()
    {
        Send();
    }
}
