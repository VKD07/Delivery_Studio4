using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    private void Awake()
    {
        //PlayerPrefs.DeleteKey("username");
    }
    public void CreateNewUserName(string userName)
    {
        PlayerPrefs.SetString("username", userName);
    }

    public bool checkIfUserExists()
    {
        return PlayerPrefs.HasKey("username");
    }

    public string GetExistingUserName => PlayerPrefs.GetString("username");

    public void SetExistingUserName()
    {
        if (ClientManager.instance.playerData.name.ToCharArray().Length > 0)
        {
            ClientManager.instance.SetPlayerName(ClientManager.instance.playerData.name);
        }
        else
        {
            ClientManager.instance.SetPlayerName(PlayerPrefs.GetString("username"));
        }
    }
}
