using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class VoiceCallManager : MonoBehaviour
{
    [SerializeField] GameObject bubbleUI;
    [SerializeField] TextMeshProUGUI bubbleMsg;
    [SerializeField] Animator voiceCallUIAnim;

    [Header("=== RANDOM CALL SETTINGS ===")]
    [SerializeField,MinMaxSlider(0, 50)] Vector2 randomCalls;

    [Header("=== DIALOUGES ===")]
    [SerializeField] float writingSpeed = .05f;
    [SerializeField] float disableTime = 5f;
    [SerializeField] string[] welcomeMessages;

    public void EnableRandomCall()
    {
        StartCoroutine(RandomCallCoroutine());
    }

    IEnumerator RandomCallCoroutine()
    {
        while (true)
        {
            float randomTime = Random.Range(randomCalls.x, randomCalls.y);
            yield return new WaitForSeconds(randomTime);
            EnableCalls();
        }
    }

    public void EnableCalls()
    {
        int index = Random.Range(0, welcomeMessages.Length);

        StartCoroutine(WritingEffect(welcomeMessages[index].ToCharArray()));
        
        bubbleUI.SetActive(true);
        voiceCallUIAnim.SetBool("ShakeIcon", true);
        StartCoroutine(DisableCall());
    }

 

    IEnumerator DisableCall()
    {
        yield return new WaitForSeconds(disableTime);
        bubbleUI.SetActive(false);
        voiceCallUIAnim.SetBool("ShakeIcon", false);
    }

    IEnumerator WritingEffect(char[] characters)
    {
        bubbleMsg.text = "";
        yield return null;

        for (int i = 0; i < characters.Length; i++)
        {
            yield return new WaitForSeconds(writingSpeed);
            bubbleMsg.text += characters[i];
        }
    }

    public void StopRandomCalls()
    {
        StopAllCoroutines();
    }
}
