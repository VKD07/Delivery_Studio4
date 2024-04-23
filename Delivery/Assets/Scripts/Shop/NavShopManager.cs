using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NavShopManager : MonoBehaviour
{
    [SerializeField] GameObject navShopPanel;
    [SerializeField] NavWallPaperItem[] wallPaperItems;
    [SerializeField] GameObject navItemPrefab;
    [SerializeField] Transform shopParent;
    [SerializeField] Image currentWallpaper;
    [SerializeField] Button closeBtn;

    [Header("=== Applied Btn ===")]
    [SerializeField] Button applyBtn;
    [SerializeField] Color appliedColor;
    Color applyBtnInitColor;

    [Header("=== CHECK IMG ===")]
    [SerializeField] GameObject checkImg;
    [SerializeField] Vector2 checkImgAdditionalPos;
    PlayerData playerData;

    //Current btn selected
    int currentWallPaperIDSelected;
    TextMeshProUGUI currentPriceTagSelected;
    Button currentPurchaseBTnSelected;

    bool defaultSelected;
    public Transform defaultItem;

    PurchaseManager purchaseManager => GetComponent<PurchaseManager>();

    private void OnEnable()
    {
        purchaseManager.PurchaseConfirmation += ApplyJustPurchasedItem;
    }

    private void OnDisable()
    {
        purchaseManager.PurchaseConfirmation -= ApplyJustPurchasedItem;
    }

    private void Start()
    {
        playerData = ClientManager.instance?.playerData;
        currentWallpaper.sprite = wallPaperItems[playerData.appliedWallpaperId].sprite;
        InitItemsToShop();
        applyBtn.onClick.AddListener(ApplyWallPaper);
        closeBtn.onClick.AddListener(CloseBtn);

        applyBtnInitColor = applyBtn.GetComponent<Image>().color;
    }

    private void InitItemsToShop()
    {
        for (int i = 0; i < wallPaperItems.Length; i++)
        {
            int index = i;
            GameObject item = Instantiate(navItemPrefab);

            wallPaperItems[index].id = index;
            item.transform.name = wallPaperItems[index].id.ToString();
            item.transform.parent = shopParent;

            item.transform.Find("WallPaper").GetComponent<Image>().sprite = wallPaperItems[index].sprite;
            item.transform.GetComponentInChildren<TextMeshProUGUI>().text = wallPaperItems[index].itemName;

            Button purchaseBtn = item.transform.Find("Purchase_BTN").GetComponent<Button>();
            TextMeshProUGUI itemPriceTxt = item.transform.Find("Purchase_BTN").GetComponentInChildren<TextMeshProUGUI>();

            //Checking if theres any owned items
            if (wallPaperItems[index].price <= 0 || CheckIfItemIsAlreadyPurchased(wallPaperItems[index].id))
            {
                itemPriceTxt.text = "OWNED";
                purchaseBtn.interactable = false;
            }
            else
            {
                itemPriceTxt.text = $"{wallPaperItems[index].price} DHS";
            }


            //ITEM PREVIEW
            item.GetComponent<Button>().onClick.AddListener(() => SelectItem(wallPaperItems[index].id, item.transform, wallPaperItems[index].sprite, purchaseBtn, itemPriceTxt));

            //PURCHASING
            purchaseBtn.onClick.AddListener(() => purchaseManager.ConfirmPurchase(wallPaperItems[index].id, ItemType.NavWallPaper, itemPriceTxt, purchaseBtn, false));

            if (playerData.appliedWallpaperId == index)
            {
                defaultItem = item.transform;
            }
        }
        StartCoroutine(SelectAppliedItem(defaultItem));
    }

    void SelectItem(int itemID, Transform btnTransform, Sprite wallpaper, Button btnSelected, TextMeshProUGUI priceTagTxt)
    {
        checkImg.SetActive(true);
        checkImg.transform.localPosition = new Vector2(btnTransform.transform.localPosition.x + checkImgAdditionalPos.x, btnTransform.transform.localPosition.y + checkImgAdditionalPos.y);
        currentWallpaper.sprite = wallpaper;
        currentWallPaperIDSelected = itemID;
        currentPurchaseBTnSelected = btnSelected;
        currentPriceTagSelected = priceTagTxt;

        ButtonIsApplied(false);
    }

    bool CheckIfItemIsAlreadyPurchased(int itemId)
    {
        if (playerData.navItemsOwned.Count > 0)
        {
            for (int i = 0; i < playerData.navItemsOwned.Count; i++)
            {
                if (playerData.navItemsOwned.Contains(itemId))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator SelectAppliedItem(Transform btnTransform)
    {
        yield return new WaitForSeconds(1.5f);

        checkImg.SetActive(true);
        checkImg.transform.localPosition = new Vector2(defaultItem.localPosition.x + checkImgAdditionalPos.x, defaultItem.localPosition.y + checkImgAdditionalPos.y);
    }

    public void ApplyWallPaper()
    {
        if (CheckIfItemIsAlreadyPurchased(currentWallPaperIDSelected) || currentWallPaperIDSelected == 0)
        {
            playerData.appliedWallpaperId = currentWallPaperIDSelected;
            ButtonIsApplied(true);
            CollectPickedMapSkin();
        }
        else //If item is not owned then ask if they wanna purchase
        {
            purchaseManager.ConfirmPurchase(wallPaperItems[currentWallPaperIDSelected].id, ItemType.NavWallPaper, currentPriceTagSelected, currentPurchaseBTnSelected, true);
        }
    }

    void ApplyJustPurchasedItem(ItemType itemType)
    {
        if (purchaseManager.ApplyItem() && itemType == ItemType.NavWallPaper)
        {
            playerData.appliedWallpaperId = currentWallPaperIDSelected;
            ButtonIsApplied(true);
            CollectPickedMapSkin();
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
            btnImg.color = applyBtnInitColor;
            applyBtnTxt.text = "APPLY";
        }
    }

    void CloseBtn()
    {
        currentPriceTagSelected = null;
        currentPurchaseBTnSelected = null;
        currentWallPaperIDSelected = 0;

        currentWallpaper.sprite = wallPaperItems[playerData.appliedWallpaperId].sprite;
        navShopPanel.SetActive(false);

        ButtonIsApplied(false);
        MainMenuCameraHandler.instance.EnableMainMenuCam();
    }

    #region Data Collection
    void CollectPickedMapSkin()
    {
        try
        {
            CollectData.instance.mapSkin = wallPaperItems[playerData.appliedWallpaperId].itemName;
        }
        catch (Exception)
        {
            Debug.Log("No Collect Data found");
        }
    }
    #endregion
}
