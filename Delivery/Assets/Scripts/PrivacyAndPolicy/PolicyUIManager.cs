using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PolicyUIManager : MonoBehaviour
{
    [SerializeField] Scrollbar scrollBar;
    [SerializeField] GameObject acceptBtn;
    [SerializeField] Button declineBtn;

    [Header("Scene Loader")]
    [SerializeField] string titleScene = "MainMenu";

    [Header("Logo Video")]
    [SerializeField] VideoPlayer logoVideo;
    [SerializeField] GameObject logoVideoTex;
    [SerializeField] float loadSceneTime;
    private void Awake()
    {
        acceptBtn.SetActive(false);
        logoVideoTex.SetActive(false);
    }

    private void Start()
    {
        acceptBtn.GetComponent<Button>().onClick.AddListener(LoadToNextScene);
        declineBtn.onClick.AddListener(ExitGame);
    }

    private void Update()
    {
        EnableAcceptButton();
    }

    private void EnableAcceptButton()
    {
        if (scrollBar.value <= .2f)
        {
            acceptBtn.SetActive(true);
        }
    }

    void LoadToNextScene()
    {
        logoVideoTex.SetActive(true);
        logoVideo.Play();
        StartCoroutine(LoadToNextSceneCoroutine());
    }

    IEnumerator LoadToNextSceneCoroutine()
    {
        yield return new WaitForSeconds(loadSceneTime);
        SceneManager.LoadScene(titleScene);
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
