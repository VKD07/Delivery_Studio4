using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarManualBtn : MonoBehaviour
{
    [SerializeField] Button carManualBtn, closeBtn;
    [SerializeField] GameObject carManualMainPanel;
    [SerializeField] UnityEvent OnManualEnabled;
    [SerializeField] UnityEvent OnManualDisabled;

    private void Start()
    {
        carManualBtn.onClick.AddListener(() => SetActiveManual());
        closeBtn.onClick.AddListener(() => SetActiveManual());
    }

    private void SetActiveManual()
    {
        if (carManualMainPanel.gameObject.activeSelf)
        {
            OnManualDisabled.Invoke();
            closeBtn.gameObject.SetActive(false);
            carManualBtn.gameObject.SetActive(true);
            carManualMainPanel.gameObject.SetActive(false);
        }
        else
        {
            OnManualEnabled.Invoke();
            closeBtn.gameObject.SetActive(true);
            carManualBtn.gameObject.SetActive(false);
            carManualMainPanel.gameObject.SetActive(true);
        }
    }

}
