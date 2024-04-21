using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
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
        //PlayerPrefs.DeleteKey("username");
    }

    #region USERNAME
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
    #endregion

    #region IP ADDRESS
    public void SaveIPAddress(string IpAddress)
    {
        PlayerPrefs.SetString("ipaddress", IpAddress);
    }

    public string GetPreviousIP()
    {
        return PlayerPrefs.GetString("ipaddress");
    }

    public bool CheckPrevIPAddress()
    {
        return PlayerPrefs.HasKey("ipaddress");
    }
    #endregion
}
