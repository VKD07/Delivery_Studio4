using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SearchService;
using UnityEngine.UI;

public class NavCustomerPackage : MonoBehaviour
{
    public static NavCustomerPackage instance;

    [SerializeField] Package[] packages;
    [SerializeField] int numberOfPackagesSpawned = 12;
    [SerializeField] Transform customerPackageContents;
    [SerializeField] GameObject customerPackagePanel;

    [Header("=== PACKAGE UI ===")]
    [SerializeField] GameObject packageBtn;
    [SerializeField] GameObject sendBtn;
    [SerializeField] Image customerPackagePanelImg;
    [SerializeField] TextMeshProUGUI panelDescription;
    [SerializeField] Animator customerPackageAnim;
    [SerializeField] Color wrongColor;
    Color initColor;


    [Header("=== INCORRECT SETTINGS ===")]
    [SerializeField] float packagePanelDisableTime = 7f;

    [Space(1)]
    [SerializeField] UnityEvent OnEnabled;

    [Header("=== LIST OF PACKAGES ===")]
    public List<Package> spawnedPackageList;
    public List<GameObject> packagesSpawned;
    [SerializeField] GameObject chosenPackageObj;
    [SerializeField] Package chosenPackageProperties;
    GameObject currentSelectedPckg;
    int packageNum = 0;

    [Space(2)]
    [SerializeField] UnityEvent OnDriverArrived;

    PackageSelectionManager packageSelection => GetComponent<PackageSelectionManager>();

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
        sendBtn.SetActive(false);
        sendBtn.GetComponent<Button>().onClick.AddListener(CheckIfRightPackageIsChosen);
        initColor = customerPackagePanelImg.color;
        packageBtn.GetComponent<Button>().onClick.AddListener(SetPackageUI);
        StartCoroutine(InitRandomCustomerPackages());
    }

    IEnumerator InitRandomCustomerPackages()
    {
        while (packageNum < numberOfPackagesSpawned)
        {
            yield return null;

            int randomPackage = Random.Range(0, packages.Length);
            packages[randomPackage].InitPackage(customerPackageContents, $"Package {packageNum}");
            packagesSpawned.Add(packages[randomPackage].spawnedPackageUI);
            spawnedPackageList.Add(packages[randomPackage]);
            packageNum++;
        }
        ChooseAPackage();
        SetPackageBtnListeners();
    }

    void SetPackageBtnListeners()
    {
        for (int i = 0; i < packagesSpawned.Count; i++)
        {
            int index = i;
            packagesSpawned[index].GetComponent<Button>().onClick.AddListener(() => SelectPackage(packagesSpawned[index]));
        }
    }

    void ChooseAPackage()
    {
        int randomPackage = Random.Range(0, spawnedPackageList.Count);
        chosenPackageProperties = spawnedPackageList[randomPackage];
        chosenPackageObj = chosenPackageProperties.spawnedPackageUI;
        SendPackets.SendChosenPackageProperties(chosenPackageProperties.name, chosenPackageProperties.packageIndex, chosenPackageProperties.tagIndex);
    }

    #region Button Listeners

    void SetPackageUI()
    {
        if (customerPackagePanel.activeSelf)
        {
            packageBtn.SetActive(false);
            customerPackagePanel.SetActive(false);
        }
        else
        {
            packageBtn.SetActive(true);
            customerPackagePanel.SetActive(true);
        }
    }

    void SelectPackage(GameObject pckg)
    {
        currentSelectedPckg = pckg;
    }

    void CheckIfRightPackageIsChosen()
    {
        if (currentSelectedPckg == null) return;

        if (currentSelectedPckg.name == chosenPackageObj.name)
        {
            RightPackageSelected();
        }
        else
        {
            WrongPackage();
        }
    }

    void WrongPackage()
    {
        sendBtn.SetActive(false);
        packageSelection.DisableAllSelection();
        customerPackageAnim.SetBool("WrongPackage", true);
        customerPackagePanelImg.color = wrongColor;
        SendPackets.SendPackageMistake();
        StartCoroutine(DisablePackageContents());

        //Sfx
        AudioManager.instance?.PlaySound("Wrong");
    }

    IEnumerator DisablePackageContents()
    {
        yield return new WaitForSeconds(.1f);
        panelDescription.gameObject.SetActive(true);
        customerPackageContents.gameObject.SetActive(false);

        yield return new WaitForSeconds(packagePanelDisableTime);
        panelDescription.gameObject.SetActive(false);
        customerPackagePanelImg.color = initColor;
        customerPackageAnim.SetBool("WrongPackage", false);
        sendBtn.SetActive(true);
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
        try
        {
            sendBtn.SetActive(true);
            customerPackageContents.gameObject.SetActive(true);
            panelDescription.gameObject.SetActive(false);
            panelDescription.color = wrongColor;
            panelDescription.text = "WRONG PACKAGE!";
            OnDriverArrived.Invoke();
        }
        catch (System.Exception)
        {

        }

    }
    #endregion
}
