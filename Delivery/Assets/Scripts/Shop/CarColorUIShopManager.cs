using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class CarColorUIShopManager : MonoBehaviour
{
    [SerializeField] Transform colorContentParent;
    [SerializeField] Material carMaterial;
    [SerializeField] GameObject colorItemPrefab;
    [SerializeField] CarColorItem defaultColor;
    [SerializeField] CarColorItem[] colorItems;

    private void Awake()
    {
        InstantiateAllColorContents();
    }

    private void Start()
    {
        SetCarTexture(defaultColor.itemTexture);
        colorItemPrefab.GetComponent<Button>().onClick.AddListener(() => SetCarTexture(defaultColor.itemTexture));
    }

    private void InstantiateAllColorContents()
    {
        for (int i = 0; i < colorItems.Length; i++)
        {
            GameObject colorItem = Instantiate(colorItemPrefab, colorContentParent.position, Quaternion.identity);
            colorItem.transform.parent = colorContentParent;
            colorItem.transform.rotation = colorContentParent.rotation;
            colorItem.GetComponent<Image>().color = colorItems[i].itemColor;
            colorItem.GetComponentInChildren<TextMeshProUGUI>().text = $"{colorItems[i].itemPrice.ToString()} AED";

            int index = i;
            colorItem.GetComponent<Button>().onClick.AddListener(() => SetCarTexture(colorItems[index].itemTexture));
        }
    }

    void SetCarTexture(Texture2D texture)
    {
        carMaterial.SetTexture("_BaseMap", texture);
    }
}
