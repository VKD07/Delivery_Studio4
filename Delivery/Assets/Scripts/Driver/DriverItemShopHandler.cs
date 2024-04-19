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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(0);
        }
    }

    public Texture2D GetPlayerChosenCarColor()
    {
        try
        {
            return colorItems[ClientManager.instance.playerData.appliedCarColoredID].itemTexture;
        }
        catch (System.Exception)
        {
            return colorItems[0].itemTexture;
        }
    }
}
