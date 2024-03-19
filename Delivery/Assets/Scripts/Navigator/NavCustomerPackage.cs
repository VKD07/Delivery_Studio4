using System.Collections;
using System.Collections.Generic;
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
        chosenPackageObj.GetComponent<Button>().onClick.AddListener(RightPackageSelected);

        SendPackets.SendChosenPackageProperties(chosenPackageProperties.name, chosenPackageProperties.packageIndex, chosenPackageProperties.tagIndex);
    }

    void RightPackageSelected()
    {
        SendPackets.SendWinPacket(ClientManager.instance.playerData.teamNumber);
        WinManager.instance?.DeclareWinner(true);
    }

    #region Network Receivers
    public void EnablePackageUI()
    {
        customerPackagePanel.SetActive(true);
    }
    #endregion
}
