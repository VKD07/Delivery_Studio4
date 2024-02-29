using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_UIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField ipAddressInput;
    public void ChooseDriver()
    {
        Client.instance.ConnectToServer(ipAddressInput.text);
        SceneManager.LoadScene(1);
    }

    public void ChooseNavigator()
    {
        Client.instance.ConnectToServer(ipAddressInput.text);
        SceneManager.LoadScene(2);
    }
}
