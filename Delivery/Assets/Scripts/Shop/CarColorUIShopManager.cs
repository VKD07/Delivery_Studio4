using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class CarColorUIShopManager : MonoBehaviour
{
    [SerializeField] Transform colorContentParent;
    [SerializeField] Material carMaterial;
    [SerializeField] GameObject colorItemPrefab;
    [SerializeField] CarColorItem defaultColor;
    [SerializeField] Button applyBTn;
    [SerializeField] Button closeBTN;
    [SerializeField] CarColorItem[] colorItems;
    int currentColorItemIdSelected;

    PlayerData playerData;
    PurchaseManager purchaseManager => GetComponent<PurchaseManager>();

    private void Awake()
    {
        playerData = ClientManager.instance?.playerData;
    }

    void Start()
    {
        applyBTn.onClick.AddListener(ApplyCarColor);
        closeBTN.onClick.AddListener(CloseBtn);
        InstantiateAllColorContents();
        SetCarTexture(colorItems[playerData.appliedCarColoredID].itemTexture, playerData.appliedCarColoredID);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(1);
        }
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
            colorItem.GetComponent<Button>().onClick.AddListener(() => SetCarTexture(colorItems[index].itemTexture, colorItems[index].id));

            //Purchase
            purchaseBtn.onClick.AddListener(() => purchaseManager.ConfirmPurchase(colorItems[index].id, ItemType.CarSkinColor, priceTag, purchaseBtn));
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


    void SetCarTexture(Texture2D texture, int itemID)
    {
        carMaterial.SetTexture("_BaseMap", texture);
        currentColorItemIdSelected = itemID;
    }

    public void ApplyCarColor()
    {
        if (CheckIfItemIsAlreadyPurchased(currentColorItemIdSelected) || currentColorItemIdSelected == 0)
        {
            playerData.appliedCarColoredID = currentColorItemIdSelected;
        }
        else
        {
            Debug.Log("Item not owned");
        }
    }

    public void CloseBtn()
    {
        SetCarTexture(colorItems[playerData.appliedCarColoredID].itemTexture, playerData.appliedCarColoredID);
        MainMenuCameraHandler.instance.EnableMainMenuCam();
    }
}
