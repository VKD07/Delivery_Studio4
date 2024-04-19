using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class NavShopManager : MonoBehaviour
{
    [SerializeField] NavWallPaperItem[] wallPaperItems;
    [SerializeField] GameObject navItemPrefab;
    [SerializeField] Transform shopParent;
    [SerializeField] Image currentWallpaper;
    [SerializeField] Button applyBtn;

    [Header("=== CHECK IMG ===")]
    [SerializeField] GameObject checkImg;
    [SerializeField] Vector2 checkImgAdditionalPos;
    PlayerData playerData;
    int currentWallPaperIDSelected;
    bool defaultSelected;
    public Transform defaultItem;

    PurchaseManager purchaseManager => GetComponent<PurchaseManager>();

    private void Start()
    {
        playerData = ClientManager.instance?.playerData;
        currentWallpaper.sprite = wallPaperItems[playerData.appliedWallpaperId].sprite;
        InitItemsToShop();
        applyBtn.onClick.AddListener(ApplyWallPaper);
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
            item.GetComponent<Button>().onClick.AddListener(() => SelectItem(wallPaperItems[index].id, item.transform, wallPaperItems[index].sprite));

            //PURCHASING
            purchaseBtn.onClick.AddListener(() => purchaseManager.ConfirmPurchase(wallPaperItems[index].id, ItemType.NavWallPaper, itemPriceTxt, purchaseBtn));

            if (playerData.appliedWallpaperId == index)
            {
                defaultItem = item.transform;
            }
        }
        StartCoroutine(SelectAppliedItem(defaultItem));
    }

    void SelectItem(int itemID, Transform btnTransform, Sprite wallpaper)
    {
        checkImg.SetActive(true);
        checkImg.transform.localPosition = new Vector2(btnTransform.transform.localPosition.x + checkImgAdditionalPos.x, btnTransform.transform.localPosition.y + checkImgAdditionalPos.y);
        currentWallpaper.sprite = wallpaper;
        currentWallPaperIDSelected = itemID;
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
        }
        else
        {
            Debug.Log("Item not owned");
        }
    }
}
