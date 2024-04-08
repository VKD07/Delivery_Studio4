using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarManualInitializer : MonoBehaviour
{
    [SerializeField] Transform panelParent;
    [Header("=== UI PREFABS ===")]
    [SerializeField] GameObject troubleShootPanel;
    [SerializeField] GameObject troubleShootPrefab;
    [SerializeField] CarTroubleShooting[] carTroubleShootings;
    Sprite prevImage;
    TextMeshProUGUI prevText;


    GameObject troubleShootUI;

    [Header("=== LIST OF PANELS ===")]
    [SerializeField] List<GameObject> troubleShootPanels;


    bool textIsUpdated;
    int currentSpawnedContent;
    bool firstIndex;
    GameObject panel;
    bool newPanel;
    void Awake()
    {
        //InitTroubleShootUI();
        StartCoroutine(InitTroubleShootPanels());
    }

    IEnumerator InitTroubleShootPanels()
    {
        while (currentSpawnedContent < carTroubleShootings.Length)
        {
            yield return null;

            if (!newPanel)
            {
                panel = Instantiate(troubleShootPanel, panelParent.position, Quaternion.identity);
                panel.transform.parent = panelParent;
                troubleShootPanels.Add(panel);
                panel.SetActive(false);
                newPanel = true;
            }

            //Make sure first panel is always enabled
            if (!firstIndex)
            {
                firstIndex = true;
                panel.SetActive(true);
            }

            InitTroubleShootUI(currentSpawnedContent, panel.transform);

            if (currentSpawnedContent < carTroubleShootings.Length)
            {
                currentSpawnedContent++;
            }

            //Make sure theres only 3 contents in one panel, else make a new panel
            if (panel.transform.childCount == 3)
            {
                newPanel = false;
            }
            else if (panel.transform.childCount <= 0)
            {
                newPanel = false;
                Destroy(panel);
                panel = null;
            }
        }

        Invoke("RemoveAllEmptyObjectsFromTheList", .5f);
    }

    void InitTroubleShootUI(int troubleShootingIndex, Transform panelParent)
    {

        //If current loop symbol is equal to prev image then dont instantiate
        textIsUpdated = false;
        if (carTroubleShootings[troubleShootingIndex].getSpriteSymbol != prevImage)
        {
            troubleShootUI = Instantiate(troubleShootPrefab, panelParent.position, Quaternion.identity);
            troubleShootUI.transform.parent = panelParent;

            Image symbol = troubleShootUI.transform.Find("CarSymbol_Img").GetComponent<Image>();
            prevImage = carTroubleShootings[troubleShootingIndex].getSpriteSymbol;
            symbol.sprite = carTroubleShootings[troubleShootingIndex].getSpriteSymbol;
        }
        else
        {
            TextMeshProUGUI textMesh = troubleShootUI.GetComponentInChildren<TextMeshProUGUI>();
            prevText = textMesh;
            prevText.text += $"\n {SetInstructionTxt(carTroubleShootings[troubleShootingIndex].GetSmokeColorEnum, carTroubleShootings[troubleShootingIndex].GetKeyCodes, carTroubleShootings[troubleShootingIndex].timeToHold)}";
            textIsUpdated = true;
        }

        if (!textIsUpdated)
        {
            TextMeshProUGUI textMesh = troubleShootUI.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = SetInstructionTxt(carTroubleShootings[troubleShootingIndex].GetSmokeColorEnum, carTroubleShootings[troubleShootingIndex].GetKeyCodes, carTroubleShootings[troubleShootingIndex].timeToHold);
            prevText = textMesh;
        }
    }

    private void RemoveAllEmptyObjectsFromTheList()
    {
        for (int i = troubleShootPanels.Count - 1; i >= 0; i--)
        {
            if (troubleShootPanels[i] == null)
            {
                troubleShootPanels.RemoveAt(i);
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
