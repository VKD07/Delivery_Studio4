using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;  

    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject ratingPanel;
    [SerializeField] float showRatingpanelDelayTime = 10f;
    [SerializeField] Image winnerBackground;
    [SerializeField] TextMeshProUGUI [] winnerTxt;
    [SerializeField] TextMeshProUGUI totalTimeTxt;
    [SerializeField] ParticleSystem[] conffettis;

    [SerializeField] UnityEvent OnWin;

    #region private var
    ClientManager thisClient;
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
        thisClient = ClientManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DeclareWinner(1);
        }
    }

    public void DeclareWinner(int teamNumber)
    {
        StartCoroutine(ShowWinnerUI(teamNumber));
        WinPanel.SetActive(true);
        TimerManager.instance?.StopTimer();
        totalTimeTxt.text = TimerManager.instance?.GetCurrentTime;
        //Time.timeScale = 0f;
        OnWin.Invoke();
    }

    public void DeclareWinner(bool value)
    {
        if (!value) return;
        TimerManager.instance?.StopTimer();
        totalTimeTxt.text = TimerManager.instance?.GetCurrentTime;
        StartCoroutine(ShowWinnerUI(thisClient.playerData.teamNumber));
        WinPanel.SetActive(true);
        //Time.timeScale = 0f;
        OnWin.Invoke();
    }

    IEnumerator ShowWinnerUI(int teamNumber)
    {
        EnableVFX();
        while(winnerBackground.fillAmount < 1)
        {
            yield return null;
            winnerBackground.fillAmount += Time.deltaTime * 5f;
        }
        yield return new WaitForSeconds(1);
        winnerTxt[0].SetText($"TEAM {teamNumber} WINS!");
        winnerTxt[1].SetText($"TEAM {teamNumber} WINS!");
        StartCoroutine(EnableRatingPanel());
    }

    IEnumerator EnableRatingPanel()
    {
        yield return new WaitForSeconds(showRatingpanelDelayTime);
        WinPanel.SetActive(false);
        ratingPanel.SetActive(true);
    }

    void EnableVFX()
    {
        for (int i = 0; i < conffettis.Length; i++)
        {
            conffettis[i].Play();
        }
    }
}