using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuCameraHandler : MonoBehaviour
{
    public static MainMenuCameraHandler instance;

    [SerializeField] GameObject mainMenuCamera, lobbyCamera, carShopCam, navShopCam;

    [SerializeField] UnityEvent OnCameraSwitch;

    MainMenuUIManager mainMenuUIManager => GetComponent<MainMenuUIManager>();
    MenuObjectsManager menuObjectsManager => GetComponent<MenuObjectsManager>();
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

    public void EnableMainMenuCam()
    {
        OnCameraSwitch.Invoke();
        mainMenuUIManager.SetActiveMainMenuPanelWithDelay();
        menuObjectsManager.SetActiveSelectionEffect(true);

        mainMenuCamera.SetActive(true);
        lobbyCamera.SetActive(false);
        navShopCam.SetActive(false);
        carShopCam.SetActive(false);
    }

    public void EnableLobbyCam()
    {
        OnCameraSwitch.Invoke();
        mainMenuCamera.SetActive(false);
        lobbyCamera.SetActive(true);
        navShopCam.SetActive(false);
        carShopCam.SetActive(false);
    }

    public void EnableCarShopCam()
    {
        OnCameraSwitch.Invoke();
        mainMenuCamera.SetActive(false);
        lobbyCamera.SetActive(false);
        navShopCam.SetActive(false);
        carShopCam.SetActive(true);
    }

    public void EnableNavShopCam()
    {
        OnCameraSwitch.Invoke();
        mainMenuCamera.SetActive(false);
        lobbyCamera.SetActive(false);
        navShopCam.SetActive(true);
        carShopCam.SetActive(false);
    }
}
