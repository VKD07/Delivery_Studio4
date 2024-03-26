using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NavCustomerPackage : MonoBehaviour
{
    public static NavCustomerPackage instance;

    [SerializeField] Package[] packages;
    [SerializeField] int numberOfPackagesSpawned = 12;
    [SerializeField] Transform customerPackageContents;
    [SerializeField] GameObject customerPackagePanel;

    [Header("=== INCORRECT SETTINGS ===")]
    [SerializeField] float packagePanelDisableTime = 7f;
    [SerializeField] GameObject wrongImg;
    [SerializeField] TextMeshProUGUI descriptionTxt;

    [Space(1)]
    [SerializeField] UnityEvent OnEnabled;

    [Header("=== LIST OF PACKAGES ===")]
    [SerializeField] List<Package> spawnedPackageList;
    [SerializeField] GameObject chosenPackageObj;
    [SerializeField] Package chosenPackageProperties;
    int packageNum = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        customerPackagePanel.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(InitRandomCustomerPackages());
    }

    IEnumerator InitRandomCustomerPackages()
    {
        while (packageNum < numberOfPackagesSpawned)
        {
            yield return null;

            int randomPackage = Random.Range(0, packages.Length);
            packages[randomPackage].InitPackage(customerPackageContents);
            packages[randomPackage].spawnedPackageUI.GetComponent<Button>().onClick.AddListener(WrongPackage);
            spawnedPackageList.Add(packages[randomPackage]);
            packageNum++;
        }
        ChooseAPackage();
    }

    void ChooseAPackage()
    {
        int randomPackage = Random.Range(0, spawnedPackageList.Count);
        chosenPackageProperties = spawnedPackageList[randomPackage];
        chosenPackageObj = chosenPackageProperties.spawnedPackageUI;
        chosenPackageObj.GetComponent<Button>().onClick.RemoveAllListeners();
        chosenPackageObj.GetComponent<Button>().onClick.AddListener(RightPackageSelected);
        SendPackets.SendChosenPackageProperties(chosenPackageProperties.name, chosenPackageProperties.packageIndex, chosenPackageProperties.tagIndex);
    }

    #region Button Listeners
    void WrongPackage()
    {
        descriptionTxt.text = "INCORRECT PACKAGE!";
        SendPackets.SendPackageMistake();
        StartCoroutine(DisablePackageContents());
    }

    IEnumerator DisablePackageContents()
    {
        yield return new WaitForSeconds(.1f);
        wrongImg.SetActive(true);
        customerPackageContents.gameObject.SetActive(false);
        yield return new WaitForSeconds(packagePanelDisableTime);
        wrongImg.SetActive(false);
        descriptionTxt.text = "CHOOSE THE RIGHT PACKAGE:";
        customerPackageContents.gameObject.SetActive(true);
    }
    void RightPackageSelected()
    {
        SendPackets.SendWinPacket(ClientManager.instance.playerData.teamNumber);
        WinManager.instance?.DeclareWinner(true);
    }
    #endregion

    #region Network Receivers
    public void EnablePackageUI()
    {
        customerPackagePanel.SetActive(true);
    }
    #endregion
}
