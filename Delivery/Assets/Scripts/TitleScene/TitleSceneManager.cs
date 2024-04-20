using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnimator;
    bool hasPressed;
    private void Awake()
    {
        //transitionAnimator.enabled = false;
    }
    private void Update()
    {
        PressAnyKeyToProceed();
    }

    private void PressAnyKeyToProceed()
    {
        if (Input.anyKeyDown && !hasPressed)
        {
            hasPressed = true;
            transitionAnimator.enabled = true;
            SceneLoaderManager.instance?.LoadNextScene(SceneLoaderManager.instance.mainMenuScene);
        }
    }
}
