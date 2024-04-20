using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NavCustomerPackage))]
public class PackageSelectionManager : MonoBehaviour
{
    NavCustomerPackage navCustomerPackage => GetComponent<NavCustomerPackage>();

    private void Start()
    {
        StartCoroutine(SetListeners());
    }

    IEnumerator SetListeners()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < navCustomerPackage.packagesSpawned.Count; i++)
        {
            int index = i;
            navCustomerPackage.packagesSpawned[index].GetComponent<Button>().onClick.AddListener(
                () => EnableSelectionUI(navCustomerPackage.packagesSpawned[index].transform.Find("SelectionUI").gameObject));
        }
    }

    void EnableSelectionUI(GameObject selectionUI)
    {
        DisableAllSelection();
        selectionUI.SetActive(true);
    }

    public void DisableAllSelection()
    {
        for (int i = 0; i < navCustomerPackage.packagesSpawned.Count; i++)
        {
            navCustomerPackage.packagesSpawned[i].transform.Find("SelectionUI").gameObject.SetActive(false);
        }
    }
}