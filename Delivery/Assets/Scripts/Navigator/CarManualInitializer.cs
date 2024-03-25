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
    TextMeshProUGUI prevText;
    GameObject troubleShootUI;
    bool textIsUpdated;
    void Awake()
    {
        InitTroubleShootUI();
    }

    void InitTroubleShootUI()
    {
        for (int i = 0; i < carTroubleShootings.Length; i++)
        {
            //If current loop symbol is equal to prev image then dont instantiate
            textIsUpdated = false;
            if (carTroubleShootings[i].getSpriteSymbol != prevImage)
            {
                troubleShootUI = Instantiate(troubleShootPrefab, troubleShootPanel.position, Quaternion.identity);
                troubleShootUI.transform.parent = troubleShootPanel.transform;

                Image symbol = troubleShootUI.transform.Find("CarSymbol_Img").GetComponent<Image>();
                prevImage = carTroubleShootings[i].getSpriteSymbol;
                symbol.sprite = carTroubleShootings[i].getSpriteSymbol;
            }
            else
            {
                TextMeshProUGUI textMesh = troubleShootUI.GetComponentInChildren<TextMeshProUGUI>();
                prevText = textMesh;
                prevText.text += $"\n {SetInstructionTxt(carTroubleShootings[i].GetSmokeColorEnum, carTroubleShootings[i].GetKeyCodes, carTroubleShootings[i].timeToHold)}";
                textIsUpdated = true;
            }
            
            if(!textIsUpdated)
            {
                TextMeshProUGUI textMesh = troubleShootUI.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.text = SetInstructionTxt(carTroubleShootings[i].GetSmokeColorEnum, carTroubleShootings[i].GetKeyCodes, carTroubleShootings[i].timeToHold);
                prevText = textMesh;
            }
        }
    }

    public string SetInstructionTxt(string colorName, KeyCode[] keysToPress, float timeToHold)
    {
        string instructions = $"If {colorName} smoke \nHold ";

        for (int i = 0; i < keysToPress.Length; i++)
        {
            instructions += keysToPress[i].ToString();

            if (i < keysToPress.Length - 1)
            {
                instructions += " + ";
            }
        }
        instructions += $" for {timeToHold} seconds";
        return instructions;
    }
}
