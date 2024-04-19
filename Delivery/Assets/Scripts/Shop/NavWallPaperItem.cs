using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallPaper", menuName = "SHOP/Add new wallpaper item")]
public class NavWallPaperItem : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite sprite;
    public float price;
}
