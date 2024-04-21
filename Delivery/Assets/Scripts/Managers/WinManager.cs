using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;

    [SerializeField] GameObject WinPanel;
    [SerializeField] Image winnerBackground;
    [SerializeField] TextMeshProUGUI winnerTxt;
    [SerializeField] ParticleSystem[] conffettis;

    [SerializeField] UnityEvent OnWin;

    [Header("=== SCENE LOADER ===")]
    [SerializeField] string sceneName = "RatingAndLeaderboard";
    [SerializeField] float timeToLoad = 5f;

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

    public void DeclareWinner(int teamNumber)
    {
        WinPanel.SetActive(true);
        winnerTxt.SetText("");
        StartCoroutine(ShowWinnerUI(teamNumber));
        TimerManager.instance?.StopTimer();
        OnWin.Invoke();
    }

    public void DeclareWinner(bool value)
    {
        if (!value) return;
        TimerManager.instance?.StopTimer();
        WinPanel.SetActive(true);
        OnWin.Invoke();

        StartCoroutine(ShowWinnerUI(thisClient.playerData.teamNumber));
    }

    IEnumerator ShowWinnerUI(int teamNumber)
    {
        EnableVFX();

        while(winnerBackground.fillAmount < 1)
        {
            yield return null;
            winnerBackground.fillAmount += Time.deltaTime * 5f;
        }

        yield return new WaitForSeconds(.5f);
        winnerTxt.SetText($"TEAM {teamNumber} WINS!");

        try
        {
            ClientManager.instance.playerData.winner = teamNumber;
            ClientManager.instance.playerData.time = TimerManager.instance?.GetCurrentTime;
        }
        catch (System.Exception)
        {
            Debug.Log("No Client");
        }
    

        StartCoroutine(LoadToRatingScene());
    }
    IEnumerator LoadToRatingScene()
    {
        yield return new WaitForSeconds(timeToLoad);
        SceneLoaderManager sceneLoader = SceneLoaderManager.instance;
        sceneLoader.LoadNextScene(sceneLoader.ratingAndLeaderBoardScene);
    }

    void EnableVFX()
    {
        for (int i = 0; i < conffettis.Length; i++)
        {
            conffettis[i].Play();
        }
    }
}