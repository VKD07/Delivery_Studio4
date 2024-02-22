using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button rightArrow, leftArrow;
    [Space]
    public UnityEvent OnRightArrowClicked;
    public UnityEvent OnLeftArrowClicked;

    void Start()
    {
        rightArrow.onClick.AddListener(OnRightBtnPressed);
        leftArrow.onClick.AddListener(OnleftBtnPRessed);
    }

    void OnRightBtnPressed()
    {
        OnRightArrowClicked.Invoke();
    }
    void OnleftBtnPRessed()
    {
        OnLeftArrowClicked.Invoke();
    }
}
