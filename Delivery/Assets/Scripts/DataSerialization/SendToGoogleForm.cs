using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogleForm : MonoBehaviour
{    
    //The link of the google form response
    private string BASE_URL = "https://docs.google.com/forms/u/2/d/e/1FAIpQLScwjJrJhm-N1-J17TPob9_op4M7WuTIp0sLUy40om63HoELAg/formResponse";

    CollectData collectData;
    private void Awake()
    {
        collectData = CollectData.instance;
    }

    //Add data inside the parenthesis
    IEnumerator Post(string crashCount, string playTime, string playerCount, string mapSkin, string carColor)
    {
        WWWForm form = new WWWForm();

        //All the input responses here
        //add inputfield ID here.
        form.AddField("entry.941594906", playTime);
        form.AddField("entry.2141905459", crashCount);
        form.AddField("entry.212434959", carColor);
        form.AddField("entry.346141718", mapSkin);
        form.AddField("entry.1989726220", playerCount);

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
        StartCoroutine(Post(collectData.crashCount.ToString(), collectData.averagePlayTime.ToString(), collectData.playerCount.ToString(), collectData.mapSkin.ToString(), collectData.carColor.ToString()));
    }

    private void OnApplicationQuit()
    {
        //Send();
    }
}
