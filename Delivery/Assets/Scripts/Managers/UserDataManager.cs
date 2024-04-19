using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteKey("username");
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
}
