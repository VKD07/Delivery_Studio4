using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_UIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TMP_InputField ipAddressInput;
    public void ChooseDriver()
    {
        Client.instance.ConnectToServer(ipAddressInput.text, playerName.text, "", GameRole.Driver);
        //SceneManager.LoadScene(1);
    }

    public void ChooseNavigator()
    {
        Client.instance.ConnectToServer(ipAddressInput.text, playerName.text, "", GameRole.Navigator);
        //SceneManager.LoadScene(2);



    }
}
