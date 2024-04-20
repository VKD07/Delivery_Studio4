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

    public delegate void PurchaseConfirmationCallBack();
    public event PurchaseConfirmationCallBack PurchaseConfirmation;

    int itemIDToPurchase;
    ItemType itemTypeToPurchase;
    TextMeshProUGUI btnTxt;
    Button purchaseBtn;
    bool applyItem;

    private void Start()
    {
        yesBtn.onClick.AddListener(YesBtn);
        noBtn.onClick.AddListener(NoBtn);
    }

    void YesBtn()
    {
        switch (itemTypeToPurchase)
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

        PurchaseConfirmation.Invoke();
    }

    void NoBtn()
    {
        purchaseConfirmationPanel.SetActive(false);
        btnTxt = null;
        itemIDToPurchase = -1;
        itemTypeToPurchase = ItemType.None;
        purchaseBtn = null;
        applyItem = false;
    }

    public void ConfirmPurchase(int itemID, ItemType itemType, TextMeshProUGUI btnText, Button purchaseBtn, bool applyItem)
    {
        purchaseConfirmationPanel.SetActive(true);
        itemIDToPurchase = itemID;
        itemTypeToPurchase = itemType;
        this.btnTxt = btnText;
        this.purchaseBtn = purchaseBtn;
        this.applyItem = applyItem;
    }

    public bool ApplyItem() => applyItem;
}

public enum ItemType
{
    None,
    NavWallPaper,
    CarSkinColor,
}
