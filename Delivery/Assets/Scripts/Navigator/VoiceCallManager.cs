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
    [SerializeField] float disableTime = 5f;
    [SerializeField] string[] welcomeMessages;

    public void EnableWelcomeCall()
    {
        int index = Random.Range(0, welcomeMessages.Length);
        bubbleMsg.text = welcomeMessages[index];
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
}
