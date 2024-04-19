using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraHandler : MonoBehaviour
{
    public static MainMenuCameraHandler instance;

    [SerializeField] GameObject mainMenuCamera, lobbyCamera, carShopCam, navShopCam;

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
        mainMenuUIManager.SetActiveMainMenuPanelWithDelay();
        menuObjectsManager.SetActiveSelectionEffect(true);

        mainMenuCamera.SetActive(true);
        lobbyCamera.SetActive(false);
        navShopCam.SetActive(false);
        carShopCam.SetActive(false);
    }

    public void EnableLobbyCam()
    {
        mainMenuCamera.SetActive(false);
        lobbyCamera.SetActive(true);
        navShopCam.SetActive(false);
        carShopCam.SetActive(false);
    }

    public void EnableCarShopCam()
    {
        mainMenuCamera.SetActive(false);
        lobbyCamera.SetActive(false);
        navShopCam.SetActive(false);
        carShopCam.SetActive(true);
    }

    public void EnableNavShopCam()
    {
        mainMenuCamera.SetActive(false);
        lobbyCamera.SetActive(false);
        navShopCam.SetActive(true);
        carShopCam.SetActive(false);
    }
}
