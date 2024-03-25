using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CustomerDialougeManager : MonoBehaviour
{
    public static CustomerDialougeManager instance;

    [SerializeField] Image packageShape, packageColor, tagColor;
    [SerializeField] GameObject customerDialougePanel, customerDialougeContents;
    [SerializeField] TextMeshProUGUI customerDialougeTxt;
    [SerializeField] Package[] packageList;

    [Header("== ANGRY CUSTOMER ===")]
    [SerializeField] float incorrectDuration = 3f;
    [SerializeField] TextMeshProUGUI angryLetters;
    string[] angryChar = { "!", "@", "$", "%", "!#", "*", "&" };
    float currentTime;
    bool start;
    CustomerAnim customerAnim;

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

    private void Update()
    {
        IncorrectTimer();
    }

    private void IncorrectTimer()
    {
        if (start && currentTime < incorrectDuration)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            start = false;
            customerDialougeContents.SetActive(true);
            angryLetters.gameObject.SetActive(false);
            customerDialougeTxt.text = "I'm looking for...";
        }
    }

    public void EnableCustomer()
    {
        StartCoroutine(ShowCustomerDialouge());
        customerAnim = FindObjectOfType<CustomerAnim>();
    }

    public void EnableAngryCustomer()
    {
        start = true;
        
        customerDialougeContents.SetActive(false);
        angryLetters.gameObject.SetActive(true);
        
        customerAnim.SwitchAnim(customerAnim.WRONGITEM);
        
        customerDialougeTxt.text = "Thats not my package!";
        
        StartCoroutine(RandomizeAngryChars());
    }

    IEnumerator ShowCustomerDialouge()
    {
        yield return new WaitForSeconds(2f);
        customerDialougePanel.SetActive(true);
    }

    IEnumerator RandomizeAngryChars()
    {
        angryLetters.text = "";
        currentTime = 0;

        while (start)
        {
            for (int i = 0; i < 5; i++)
            {
                int randomChar = Random.Range(0, angryChar.Length);
                angryLetters.text += angryChar[randomChar];
            }

            yield return new WaitForSeconds(1f);

            angryLetters.text = "";
        }
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
