using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarManualInitializer : MonoBehaviour
{
    [SerializeField] Transform troubleShootPanel;
    [SerializeField] GameObject troubleShootPrefab;
    [SerializeField] CarTroubleShooting[] carTroubleShootings;
    Sprite prevImage;
    void Awake()
    {
        InitTroubleShootUI();
    }

    void InitTroubleShootUI()
    {
        for (int i = 0; i < carTroubleShootings.Length; i++)
        {
            //if (carTroubleShootings[i].getSpriteSymbol == prevImage) continue;
            GameObject troubleShootUI = Instantiate(troubleShootPrefab, troubleShootPanel.position, Quaternion.identity);
            troubleShootUI.transform.parent = troubleShootPanel.transform;
            Image symbol = troubleShootUI.transform.Find("CarSymbol_Img").GetComponent<Image>();
            prevImage = carTroubleShootings[i].getSpriteSymbol;
            symbol.sprite = carTroubleShootings[i].getSpriteSymbol;
            troubleShootUI.GetComponentInChildren<TextMeshProUGUI>().text = carTroubleShootings[i].GetInstructionsTxt();
        }
    }
}
