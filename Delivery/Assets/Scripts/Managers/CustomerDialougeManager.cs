using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomerDialougeManager : MonoBehaviour
{
    public static CustomerDialougeManager instance;

    [SerializeField] Image packageShape, packageColor, tagColor;
    [SerializeField] GameObject customerDialougePanel;

    [SerializeField] Package[] packageList; 

    #region Setters 
    //public void SetShapeImage(Sprite shape) => packageShape.sprite = shape;
    //public void SetPackageColor(Color color) => packageColor.color = color;
    //public void SetTagColor(Color color) => tagColor.color = color;
    #endregion

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
    }

    public void EnableCustomer()
    {
        StartCoroutine(ShowCustomerDialouge());
    }

    IEnumerator ShowCustomerDialouge()
    {
        yield return new WaitForSeconds(2f);
        customerDialougePanel.SetActive(true);
    }

    #region Network Receivers
    public void EnableAndSetCustomerDialouge(string packageName, int packageIndexColor, int tagIndexColor)
    {
        for (int i = 0; i < packageList.Length; i++)
        {
            if (packageList[i].name == packageName)
            {
                packageShape.sprite = packageList[i].packageShape;
                packageColor.color = packageList[i].packageColor[packageIndexColor];
                tagColor.color = packageList[i].tagColors[tagIndexColor];
            }
        }
    }
    #endregion
}
