using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavShopItemHandler : MonoBehaviour
{
    [Header("=== WALLPAPER ===")]
    [SerializeField] NavWallPaperItem[] navWallpaperItems;
    [SerializeField] SpriteRenderer currentWallPaper;
    private void Awake()
    {
        ApplyAllItemsChosen();
    }
    void ApplyAllItemsChosen()
    {
        currentWallPaper.sprite = navWallpaperItems[ClientManager.instance.playerData.appliedWallpaperId].sprite;
    }
}
