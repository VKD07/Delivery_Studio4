using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DriverItemShopHandler : MonoBehaviour
{
    public static DriverItemShopHandler instance;
    [Header("=== CAR COLOR ===")]
    [SerializeField] CarColorItem[] colorItems;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this);
        }
    }

    public Texture2D GetPlayerChosenCarColor(int colorID)
    {
        try
        {
            return colorItems[colorID].itemTexture;
        }
        catch (System.Exception)
        {
            return colorItems[0].itemTexture;
        }
    }
}
