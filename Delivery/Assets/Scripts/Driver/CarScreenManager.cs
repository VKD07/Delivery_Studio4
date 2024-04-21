using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarScreenManager : MonoBehaviour
{
    public static CarScreenManager instance;

    [Header("=== UI PANELS ===")]
    [SerializeField] GameObject carScreenPanel;
    [SerializeField] GameObject homeScreenUI;
    [SerializeField] GameObject carSystemPanel;
    [SerializeField] GameObject healthyPanel;
    [SerializeField] GameObject malfunctionPanel;
    [SerializeField] GameObject warningUI;

    [Header("=== BUTTONS ===")]
    [SerializeField] Button winShieldBtn;
    [SerializeField] Button systemBtn;
    [SerializeField] Button homeBtn;

    public Image carMalfunctionSymbol;
    bool carIsDamaged;

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

    void Start()
    {
        warningUI.SetActive(false);
        winShieldBtn.onClick.AddListener(WinShieldBtn);
        systemBtn.onClick.AddListener(SystemBtn);
        homeBtn.onClick.AddListener(HomeBtn);
    }

    void WinShieldBtn()
    {
        SendPackets.SendWiper();
        MudSplashManager.instance?.WipeOffMud();
    }

    void SystemBtn()
    {
        if(carIsDamaged)
        {
            carMalfunctionSymbol.enabled = true;
        }
        homeBtn.gameObject.SetActive(true);
        homeScreenUI.SetActive(false);
        carSystemPanel.SetActive(true);
    }

    void HomeBtn()
    {
        carMalfunctionSymbol.enabled = false;
        homeBtn.gameObject.SetActive(false);
        carSystemPanel.SetActive(false);
        homeScreenUI.SetActive(true);
    }

    public void CarIsDamaged(bool val)
    {
        if(val)
        {
            carIsDamaged = true;
            warningUI.SetActive(true);
            healthyPanel.SetActive(false);
            malfunctionPanel.SetActive(true);
        }
        else
        {
            carIsDamaged = false;
            warningUI.SetActive(false);
            carMalfunctionSymbol.enabled = false;
            healthyPanel.SetActive(true);
            malfunctionPanel.SetActive(false);
        }
    }

    public void DisableCarScreen()
    {
        carScreenPanel.SetActive(false);
    }
}
