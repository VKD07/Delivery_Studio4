using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoiceCallManager : MonoBehaviour
{
    [SerializeField] GameObject bubbleUI;
    [SerializeField] TextMeshProUGUI bubbleMsg;
    [SerializeField] Animator voiceCallUIAnim;

    //[Header("=== RANDOM CALL SETTINGS ===")]


    [Header("=== DIALOUGES ===")]
    [SerializeField] float writingSpeed = .05f;
    [SerializeField] float disableTime = 5f;
    [SerializeField] string[] welcomeMessages;

    public void EnableWelcomeCall()
    {
        int index = Random.Range(0, welcomeMessages.Length);

        StartCoroutine(WritingEffect(welcomeMessages[index].ToCharArray()));
        
        bubbleUI.SetActive(true);
        voiceCallUIAnim.SetBool("ShakeIcon", true);
        StartCoroutine(DisableWelcomeCall());
    }

    IEnumerator DisableWelcomeCall()
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
}
