using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPackage : MonoBehaviour
{
    public GameObject selectionUI;

    Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
        //btn.onClick?.AddListener(EnableSelectionUI);
    }

    void EnableSelectionUI()
    {
        //NavCustomerPackage.instance.DisableAllSelection();
        //selectionUI.SetActive(true);
    }
}
