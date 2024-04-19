using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RatingUIManager : MonoBehaviour
{
    public static RatingUIManager instance;

    [Header("=== PANEL UI ===")]
    [SerializeField] GameObject rateYourPartnerPanel;
    [SerializeField] GameObject overallRatingPanel;
    [SerializeField] GameObject fourPlayersRatingPanel;
    [SerializeField] GameObject twoPlayersRatingPanel;

    [Header("=== 2 player Rating UI ===")]
    [SerializeField] TextMeshProUGUI timerTxt;
    [NonReorderable]
    [SerializeField] PlayerAndRating[] DuoPlayerRatingUI;

    [Header("=== 4 player Rating UI ===")]
    [SerializeField] TextMeshProUGUI winnerTxt;
    [SerializeField] TextMeshProUGUI timerTxt2;
    [NonReorderable]
    [SerializeField] PlayerAndRating[] twoVTwoPlayerRatingUI;

    [Header("=== BACKGROUND ===")]
    [SerializeField] Image ratingBackground;
    Material bgMat;
    public float scrollSpeed = 0.5f;

    private void Awake()
    {
        SingleTon();
        rateYourPartnerPanel.SetActive(true);
        overallRatingPanel.SetActive(false);
    }

    void Start()
    {
        bgMat = ratingBackground.material;
        SetWinnerAndTimer();
    }

    void Update()
    {
        ScrollingEffect();
    }

    void SingleTon()
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
    private void SetWinnerAndTimer()
    {
        timerTxt.text = ClientManager.instance?.playerData.time;
        timerTxt2.text = ClientManager.instance?.playerData.time;
        winnerTxt.text = $"TEAM {ClientManager.instance?.playerData.winner} WINS!";
    }

    private void ScrollingEffect()
    {
        float offset = Time.time * scrollSpeed;
        bgMat.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }

    private void OnDisable()
    {
        bgMat.SetTextureOffset("_MainTex", new Vector2(0, 0));
    }

    public void EnableOverAllRating()
    {
        rateYourPartnerPanel.SetActive(false);
        overallRatingPanel.SetActive(true);

        switch (ClientManager.instance?.playerData.mode)
        {
            case LobbyMode.Duo:
                twoPlayersRatingPanel.SetActive(true);
                fourPlayersRatingPanel.SetActive(false);
                LeaderboardUIManager.instance.SendTeamDataToDB();
                break;

            case LobbyMode.TwoVTwo:
                twoPlayersRatingPanel.SetActive(false);
                fourPlayersRatingPanel.SetActive(true);
                break;
        }
    }

    public void SetRating(GameObject starPanel, int rating, int index)
    {
        //SetStars InOutEffect
        if (ClientManager.instance?.playerData.mode == LobbyMode.TwoVTwo)
        {
            StartCoroutine(EaseInOutEffect(twoVTwoPlayerRatingUI[index].playerRatingPanel, 1.1768f, 1f, .5f));
        }
        else if (ClientManager.instance?.playerData.mode == LobbyMode.Duo)
        {
            StartCoroutine(EaseInOutEffect(DuoPlayerRatingUI[index].playerRatingPanel, 1.1768f, 1f, .5f));
        }

        for (int i = 0; i < rating; i++)
        {
            starPanel.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
        }
    }

    private IEnumerator EaseInOutEffect(Transform targetTransform, float startScale, float endScale, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            targetTransform.localScale = Vector3.Lerp(startScale * Vector3.one, endScale * Vector3.one, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = endScale * Vector3.one;
    }

    #region Network Receivers
    public void ReceiveNamesAndRatings(string[] playerNames, int[] starRating)
    {
        switch (ClientManager.instance?.playerData.mode)
        {
            case LobbyMode.TwoVTwo:

                for (int i = 0; i < playerNames.Length; i++)
                {
                    int index = i;

                    twoVTwoPlayerRatingUI[index].playerName.text = playerNames[index];
                    SetRating(twoVTwoPlayerRatingUI[index].starPanel, starRating[index], index);
                }

                break;

            case LobbyMode.Duo:

                for (int i = 0; i < playerNames.Length; i++)
                {
                    int index = i;

                    DuoPlayerRatingUI[index].playerName.text = playerNames[index];
                    SetRating(DuoPlayerRatingUI[index].starPanel, starRating[index], index);
                }
                break;
        }

    }
    #endregion
}

[System.Serializable]
public class PlayerAndRating
{
    public Transform playerRatingPanel;
    public TextMeshProUGUI playerName;
    public GameObject starPanel;
}
