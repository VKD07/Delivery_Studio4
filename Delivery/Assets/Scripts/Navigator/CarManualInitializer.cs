using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarManualInitializer : MonoBehaviour
{
    [SerializeField] Transform carManualParent;

    [Header("=== UI PREFABS ===")]
    [SerializeField] GameObject manualPage;
    [SerializeField] GameObject troubleShootPanel;
    [SerializeField] GameObject troubleShootPrefab;
    [SerializeField] CarTroubleShooting[] carTroubleShootings;

    [Header("=== LIST OF PANELS ===")]
    public List<GameObject> pages;

    bool textIsUpdated;
    int currentSpawnedContent;
    bool firstIndex;
    Sprite prevImage;
    TextMeshProUGUI prevText;
    GameObject troubleShootUI;
    GameObject page;
    GameObject manualContent;
    bool newPanel;
    void Awake()
    {
        StartCoroutine(InitTroubleShootPanels());
    }

    IEnumerator InitTroubleShootPanels()
    {
        while (currentSpawnedContent < carTroubleShootings.Length)
        {
            yield return null;

            if (!newPanel)
            {
                page = Instantiate(manualPage);
                page.transform.parent = carManualParent;
                page.transform.localPosition = new Vector3(0, 0.9f, 0);

                manualContent = Instantiate(troubleShootPanel);
                Transform manualContentPanel = page.transform.Find("CAR MANUAL CANVAS (WORLDSPACE)").transform.Find("Manual_Contents").transform;
                manualContent.GetComponent<RectTransform>().SetParent(manualContentPanel, false);

                pages.Add(page);
                page.SetActive(false);
                newPanel = true;
            }

            //Make sure first panel is always enabled
            if (!firstIndex)
            {
                page.transform.localPosition = Vector3.up;
                firstIndex = true;
                page.SetActive(true);
            }

            InitTroubleShootUI(currentSpawnedContent, manualContent.transform);

            if (currentSpawnedContent < carTroubleShootings.Length)
            {
                currentSpawnedContent++;
            }

            //Make sure theres only 3 contents in one panel, else make a new panel
            if (manualContent.transform.childCount == 3)
            {
                newPanel = false;
            }
            else if (manualContent.transform.childCount <= 0)
            {
                newPanel = false;
                Destroy(page);
                page = null;
                manualContent = null;
            }
        }

        Invoke("RemovePageWithEmptyContents", .1f);
    }

    void InitTroubleShootUI(int troubleShootingIndex, Transform panelParent)
    {

        //If current loop symbol is equal to prev image then dont instantiate
        textIsUpdated = false;
        if (carTroubleShootings[troubleShootingIndex].getSpriteSymbol != prevImage)
        {
            troubleShootUI = Instantiate(troubleShootPrefab);
            troubleShootUI.GetComponent<RectTransform>().SetParent(panelParent, false);

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

    private void RemovePageWithEmptyContents()
    {
        for (int i = pages.Count - 1; i >= 0; i--)
        {
            if (pages[i] == null)
            {
                pages.RemoveAt(i);
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
