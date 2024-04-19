using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] GameObject purchaseConfirmationPanel;

    [SerializeField] Button yesBtn, noBtn;

    int itemIDToPurchase;
    ItemType itemTypeToPurchase;
    TextMeshProUGUI btnTxt;
    Button purchaseBtn;

    private void Start()
    {
        yesBtn.onClick.AddListener(YesBtn);
        noBtn.onClick.AddListener(NoBtn);
    }

    void YesBtn()
    {
        switch(itemTypeToPurchase)
        {
            case ItemType.NavWallPaper:
                ClientManager.instance?.playerData.navItemsOwned.Add(itemIDToPurchase);
                break;
            case ItemType.CarSkinColor:
                ClientManager.instance?.playerData.carColorOwned.Add(itemIDToPurchase);
                break;
        }
        btnTxt.text = "OWNED";
        purchaseBtn.interactable = false;
        purchaseConfirmationPanel.SetActive(false);
    }

    void NoBtn()
    {
        purchaseConfirmationPanel.SetActive(false);
        btnTxt = null;
        itemIDToPurchase = -1;
        itemTypeToPurchase = ItemType.None;
        purchaseBtn = null;
    }

    public void ConfirmPurchase(int itemID, ItemType itemType, TextMeshProUGUI btnText, Button purchaseBtn)
    {
        purchaseConfirmationPanel.SetActive(true);
        itemIDToPurchase = itemID;
        itemTypeToPurchase = itemType;
        this.btnTxt = btnText;
        this.purchaseBtn = purchaseBtn;
    }
}

public enum ItemType
{
    None,
    NavWallPaper,
    CarSkinColor,
}
