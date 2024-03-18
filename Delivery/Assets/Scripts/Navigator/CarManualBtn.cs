using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarManualBtn : MonoBehaviour
{
    [SerializeField] Button carManualBtn, closeBtn;
    [SerializeField] GameObject carManualUIPanel;
    [SerializeField] UnityEvent OnManualEnabled;
    [SerializeField] UnityEvent OnManualDisabled;

    //[Header("=== HOVER EFFECT ===")]
    //[SerializeField] float scaleSpeed = 5f;
    //[SerializeField] float targetScale = .2f;
    //Vector3 initScale;

    //void Awake()
    //{
    //    initScale = carManualBtn.gameObject.transform.localScale;
    //}

    private void Start()
    {
        carManualBtn.onClick.AddListener(() => SetActiveManualUI());
        closeBtn.onClick.AddListener(() => SetActiveCloseBtn());
    }

    void SetActiveManualUI()
    {
        if (!carManualUIPanel.activeSelf)
        {
            OnManualEnabled.Invoke();
            carManualUIPanel.SetActive(true);
            carManualBtn.gameObject.SetActive(false);
        }
    }

    void SetActiveCloseBtn()
    {
        if (carManualUIPanel.activeSelf)
        {
            OnManualDisabled.Invoke();
            carManualUIPanel.SetActive(false);
            carManualBtn.gameObject.SetActive(true);
        }
    }

    #region Event Triggers
    public void OnPointerEnter()
    {
    }

    public void OnPointerExit()
    {
    }
    #endregion
}
