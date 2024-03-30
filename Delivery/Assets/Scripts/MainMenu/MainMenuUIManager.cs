using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel, mainMenuPanel, newUserPanel, contractPanel;

    [Header("=== NEW USER PANEL ===")]
    [SerializeField] Button yesBtn;
    [SerializeField] Button noBtn;
    [SerializeField] UnityEvent OnYesClick;
    [SerializeField] UnityEvent OnNoClick;

    [Space(2)]
    [Header("=== CONTRACT PANEL ===")]
    public TMP_InputField userNameInput;
    public TextMeshProUGUI placeHolderTxt;

    [Space(2)]
    [Header("=== USERNAME CONFIRMATION ===")]
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] Button confirmBtn, noConfirmBtn;
    [SerializeField] UnityEvent confirmBtnClick;
    [SerializeField] UnityEvent noConfirmBtnClick;


    void Awake()
    {
        SetActiveTitlePanel(true);
        ButtonListeners();
    }

    private void ButtonListeners()
    {
        yesBtn.onClick.AddListener(NewUserBtn);
        noBtn.onClick.AddListener(NoNewUserBtn);

        confirmBtn.onClick.AddListener(ConfirmUserNameBtn);
        noConfirmBtn.onClick.AddListener(NotConfirmedUserNameBtn);

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

    #region BtnListeners Func
    void NewUserBtn()
    {
        OnYesClick.Invoke();
        newUserPanel.SetActive(false);
    }

    void NoNewUserBtn()
    {
        OnNoClick.Invoke();
        SetActiveMainMenuPanel();
    }

    void ConfirmUserNameBtn()
    {
        SetActiveMainMenuPanel();
        confirmationPanel.SetActive(false);
        confirmBtnClick.Invoke();
    }

    void NotConfirmedUserNameBtn()
    {
        confirmationPanel.SetActive(false);
        noConfirmBtnClick.Invoke();
    }

    #endregion

    #region Setters Func
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
        contractPanel.SetActive(true);
    }

    public void SetActiveUserNameConfirmationPanel(bool val)
    {
        confirmationPanel.SetActive(val);
    }
    #endregion
}
