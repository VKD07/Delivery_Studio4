using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CarColorUIShopManager : MonoBehaviour
{
    [SerializeField] Transform colorContentParent;
    [SerializeField] Material carMaterial;
    [SerializeField] GameObject colorItemPrefab;
    [SerializeField] CarColorItem defaultColor;
    [SerializeField] Button closeBTN;
    [SerializeField] CarColorItem[] colorItems;

    //Current Selected item
    int currentColorItemIdSelected;
    Button currentSelectedPurchaseBtn;
    TextMeshProUGUI currentPriceTagSelected;

    [Header("APPLY BTN")]
    [SerializeField] Button applyBtn;
    [SerializeField] Color appliedColor;
    Color applyInitBtnColor;

    PlayerData playerData;
    PurchaseManager purchaseManager => GetComponent<PurchaseManager>();


    private void OnEnable()
    {
        purchaseManager.PurchaseConfirmation += ApplyJustPurchasedItem;
    }

    private void OnDisable()
    {
        purchaseManager.PurchaseConfirmation -= ApplyJustPurchasedItem;
    }

    void Start()
    {
        applyInitBtnColor = applyBtn.GetComponent<Image>().color;

        playerData = ClientManager.instance.playerData;
        applyBtn.onClick.AddListener(ApplyCarColor);
        closeBTN.onClick.AddListener(CloseBtn);
        InstantiateAllColorContents();
        SetCarTexture(colorItems[playerData.appliedCarColoredID].itemTexture, playerData.appliedCarColoredID, null);
    }

    private void InstantiateAllColorContents()
    {
        for (int i = 0; i < colorItems.Length; i++)
        {
            int index = i;

            GameObject colorItem = Instantiate(colorItemPrefab, colorContentParent.position, Quaternion.identity);
            colorItem.transform.parent = colorContentParent;
            colorItem.transform.rotation = colorContentParent.rotation;

            //Setting item ID
            colorItems[index].id = index;


            colorItem.GetComponentInChildren<Image>().color = colorItems[index].itemColor;

            Button purchaseBtn = colorItem.transform.Find("PurchaseBtn").GetComponent<Button>();
            TextMeshProUGUI priceTag = colorItem.GetComponentInChildren<TextMeshProUGUI>();

            if (colorItems[index].itemPrice <= 0 || CheckIfItemIsAlreadyPurchased(colorItems[index].id))
            {
                priceTag.text = $"OWNED";
                purchaseBtn.interactable = false;
            }
            else
            {
                priceTag.text = $"{colorItems[index].itemPrice.ToString()} AED";
            }

            //Set Shop items btn listeners
            colorItem.GetComponent<Button>().onClick.AddListener(() => SetCarTexture(colorItems[index].itemTexture, colorItems[index].id, colorItem));

            //Purchase
            purchaseBtn.onClick.AddListener(() => purchaseManager.ConfirmPurchase(colorItems[index].id, ItemType.CarSkinColor, priceTag, purchaseBtn, false));
        }
    }

    bool CheckIfItemIsAlreadyPurchased(int itemId)
    {
        if (playerData.carColorOwned.Count > 0)
        {
            for (int i = 0; i < playerData.carColorOwned.Count; i++)
            {
                if (playerData.carColorOwned.Contains(itemId))
                {
                    return true;
                }
            }
        }
        return false;
    }


    void SetCarTexture(Texture2D texture, int itemID, GameObject colorItem)
    {

        carMaterial.SetTexture("_BaseMap", texture);

        currentColorItemIdSelected = itemID;

        ButtonIsApplied(false);

        if (colorItem == null) return;
        currentSelectedPurchaseBtn = colorItem.transform.Find("PurchaseBtn").GetComponent<Button>();
        currentPriceTagSelected = colorItem.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ApplyCarColor()
    {
        if (CheckIfItemIsAlreadyPurchased(currentColorItemIdSelected) || currentColorItemIdSelected == 0)
        {
            ButtonIsApplied(true);
            playerData.appliedCarColoredID = currentColorItemIdSelected;
            CollectColorPicked();
        }
        else
        {
            purchaseManager.ConfirmPurchase(colorItems[currentColorItemIdSelected].id, ItemType.CarSkinColor, currentPriceTagSelected, currentSelectedPurchaseBtn, true);
        }
    }

    void ApplyJustPurchasedItem(ItemType itemType)
    {
        if (purchaseManager.ApplyItem() && itemType == ItemType.CarSkinColor)
        {
            playerData.appliedCarColoredID = currentColorItemIdSelected;
            CollectColorPicked();
            ButtonIsApplied(true);
        }
    }

    void ButtonIsApplied(bool val)
    {
        TextMeshProUGUI applyBtnTxt = applyBtn.GetComponentInChildren<TextMeshProUGUI>();
        Image btnImg = applyBtn.GetComponent<Image>();

        if (val)
        {
            btnImg.color = appliedColor;
            applyBtnTxt.text = "APPLIED";
        }
        else
        {
            btnImg.color = applyInitBtnColor;
            applyBtnTxt.text = "APPLY";
        }
    }

    public void CloseBtn()
    {
        ButtonIsApplied(false);
        SetCarTexture(colorItems[playerData.appliedCarColoredID].itemTexture, playerData.appliedCarColoredID, null);
        currentColorItemIdSelected = 0;
        currentSelectedPurchaseBtn = null;
        currentPriceTagSelected = null;
        MainMenuCameraHandler.instance.EnableMainMenuCam();
    }

    #region DataCollection
    void CollectColorPicked()
    {
        try
        {
            CollectData.instance.carColor = colorItems[playerData.appliedCarColoredID].itemTexture.name;
        }
        catch (Exception)
        {
            Debug.Log("No Collect Data found");
        }
    }
    #endregion
}
