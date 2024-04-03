using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel, mainMenuPanel, newUserPanel, contractPanel, lobbySelectionPanel, shopSelectionPanel;

    [Space(2)]
    [Header("=== CONTRACT PANEL ===")]
    public TMP_InputField userNameInput;
    public TextMeshProUGUI placeHolderTxt;

    [Space(2)]
    [Header("=== USERNAME CONFIRMATION ===")]
    [SerializeField] public GameObject confirmationPanel;

    [Space(3)]
    [Header("=== PANELS AND EVENTS ===")]
    [SerializeField] SelectionPanel[] selectionPanels;


    void Awake()
    {
        SetActiveTitlePanel(true);
        InitPanelsAndBtnEvents();
    }

    private void InitPanelsAndBtnEvents()
    {
        for (int i = 0; i < selectionPanels.Length; i++)
        {
            for (int j = 0; j < selectionPanels[i].btnAndEvents.Length; j++)
            {
                ButtonAndEvents buttonAndEvents = selectionPanels[i].btnAndEvents[j];
                buttonAndEvents.button.onClick.AddListener(() => buttonAndEvents.btnEvent.Invoke());
            }
        }
      
    }

    private void Update()
    {
        PressAnyKey();
    }

    private void PressAnyKey()
    {
        if (Input.anyKey && titlePanel.activeSelf)
        {
            SetActiveNewUserPanel();
            SetActiveTitlePanel(false);
        }
    }

    #region UI Setters Func
    public void SetActiveTitlePanel(bool val)
    {
        if (!val)
        {
            titlePanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            newUserPanel.SetActive(true);
            contractPanel.SetActive(false);
        }
        else
        {
            titlePanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            newUserPanel.SetActive(false);
            contractPanel.SetActive(false);
        }
    }

    public void SetActiveMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        titlePanel.SetActive(false);
        newUserPanel.SetActive(false);
        contractPanel.SetActive(false);
    }

    public void SetActiveNewUserPanel()
    {
        titlePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        contractPanel.SetActive(false);
        newUserPanel.SetActive(true);
    }

    public void SetActiveContractPanel()
    {
        titlePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        newUserPanel.SetActive(false);
    }

    public void SetActiveContractPanelWithDelay()
    {
        contractPanel.SetActive(true);
    }

    public void SetActiveLobbySelectionPanel(bool val)
    {
        lobbySelectionPanel.SetActive(val);
    }

    public void SetActiveMainMenuPanel(bool val)
    {
        mainMenuPanel.SetActive(val);
    }

    public void SetActiveUserNameConfirmationPanel(bool val)
    {
        confirmationPanel.SetActive(val);
    }
    
    public void SetActiveMainMenuPanelWithDelay()
    {
        StartCoroutine(SetActiveMenuPanel());
    }

    IEnumerator SetActiveMenuPanel()
    {
        yield return new WaitForSeconds(1.5f);
        SetActiveMainMenuPanel();
    }
    #endregion
}

[System.Serializable]
public class SelectionPanel
{
    public string panelName;
    public ButtonAndEvents[] btnAndEvents;
}

[System.Serializable]
public class ButtonAndEvents
{
    public string eventName;
    public Button button;
    public UnityEvent btnEvent;
}