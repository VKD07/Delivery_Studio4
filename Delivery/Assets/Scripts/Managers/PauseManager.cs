using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] GameObject pausePanel, gameIsPausedTxt;
    [SerializeField] Button resumeBtn, leaveBtn;
    [SerializeField] AudioSource musicSource;

    [Header("=== EVENTS ===")]
    [SerializeField] UnityEvent OnPause;
    [SerializeField] UnityEvent UnPause;
    bool otherPlayerHasPaused;
    bool isPaused;

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

    private void Start()
    {
        resumeBtn.onClick.AddListener(ResumeGame);
        leaveBtn.onClick.AddListener(LeaveGame);
    }

    private void Update()
    {
        PauseTheGame();
    }

    private void PauseTheGame()
    {
        if (Input.GetKeyDown(pauseKey) && !otherPlayerHasPaused)
        {
            if (!isPaused)
            {
                SendPackets.SendPausePacket(true);
                isPaused = true;
                Time.timeScale = 0;
                OnPause.Invoke();
            }
        }
    }

    void ResumeGame()
    {
        if (isPaused)
        {
            SendPackets.SendPausePacket(false);
            isPaused = false;
            Time.timeScale = 1;
            UnPause.Invoke();
        }
    }

    void LeaveGame()
    {
        SendPackets.SendPlayerLeft(ClientManager.instance?.playerData.name);
        Time.timeScale = 1;
        SceneLoaderManager sceneLoader = SceneLoaderManager.instance;
        ClientManager.instance?.playerData.ClearData();
        HandlePackets.newRatingData = false;
        sceneLoader.LoadNextScene(sceneLoader.mainMenuScene);
    }

    #region NETWORK Receivers
    public void ReceivePauseFromTheNetwork(bool isPaused)
    {
        if (isPaused)
        {
            musicSource.Pause();
            otherPlayerHasPaused = true;
            pausePanel.SetActive(true);
            gameIsPausedTxt.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            musicSource.Play();
            otherPlayerHasPaused = false;
            pausePanel.SetActive(false);
            gameIsPausedTxt.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ReceivePlayerLeft(string playerWhoLeft)
    {
        gameIsPausedTxt.GetComponent<TextMeshProUGUI>().text = $"{playerWhoLeft} Left.";
        Time.timeScale = 1;
        SceneLoaderManager sceneLoader = SceneLoaderManager.instance;
        ClientManager.instance?.playerData.ClearData();
        HandlePackets.newRatingData = false;
        sceneLoader.LoadNextScene(sceneLoader.mainMenuScene);
    }
    #endregion
}
