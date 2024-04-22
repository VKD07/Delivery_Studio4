using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager instance;
    [SerializeField] Animator transitionAnimator;
    [SerializeField] float loadSceneDelayTime;
    [SerializeField] float currentSceneDelayTime;
    [SerializeField] bool enableCurrentSceneTranstionEffect;

    [Header("=== SCENE NAMES ===")]
    public string titleScene;
    public string mainMenuScene = "MainMenu";
    public string driverScene;
    public string navigatorScene;
    public string ratingAndLeaderBoardScene = "RatingAndLeaderboard";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance !=  this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (enableCurrentSceneTranstionEffect)
        {
            AudioManager.instance?.PlaySound("ScreenAppear");
            StartCoroutine(TriggerCurrentSceneTransitionEffect());
        }
    }

    IEnumerator TriggerCurrentSceneTransitionEffect()
    {
        yield return new WaitForSeconds(currentSceneDelayTime);
        transitionAnimator.SetTrigger("CurrentScene");
    }

    public void LoadNextScene(string sceneName)
    {
        AudioManager.instance?.PlaySound("ScreenTransition");
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }

    IEnumerator LoadSceneWithDelay(string sceneName)
    {
        transitionAnimator.SetTrigger("NextScene");
        yield return new WaitForSeconds(loadSceneDelayTime);
        SceneManager.LoadScene(sceneName);
    }
}
