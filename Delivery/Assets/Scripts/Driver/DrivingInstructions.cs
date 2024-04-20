using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingInstructions : MonoBehaviour
{
    [SerializeField] GameObject instructionImg;
    [SerializeField] float blinkSpeed = 1f;
    [SerializeField] float timeToDisable = 10f;
    Image img;

    private void Awake()
    {
        img = instructionImg.GetComponent<Image>();
    }
    void Start()
    {
        StartCoroutine(DisableTime());
    }

    private void Update()
    {
        EnableBlinkingEffect();
    }

    void EnableBlinkingEffect()
    {
        if (instructionImg.activeSelf)
        {
            float alpha = Mathf.Lerp(0.2f, 1, Mathf.Sin(Time.time * blinkSpeed));
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
        }
    }

    IEnumerator DisableTime()
    {
        yield return new WaitForSeconds(timeToDisable);
        instructionImg.SetActive(false);
    }
}
