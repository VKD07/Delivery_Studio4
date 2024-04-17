using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingUIManager : MonoBehaviour
{
    public static RatingUIManager instance;

    [Header("=== PANEL UI ===")]
    [SerializeField] GameObject ratingPanel;
    [SerializeField] GameObject rateYourPartnerPanel;
    [SerializeField] GameObject overallRatingPanel;

    [Header("=== Player Rating UI ===")]
    [SerializeField] TextMeshProUGUI[] playerNames;
    [SerializeField] GameObject[] starPanels;
    [SerializeField] Transform [] playerRatingPanel;

    [Header("=== BACKGROUND ===")]
    [SerializeField] Image ratingBackground;
    Material bgMat;
    public float scrollSpeed = 0.5f;

    private void Awake()
    {
        SingleTon();
        ratingPanel.SetActive(false);
        rateYourPartnerPanel.SetActive(true);
        overallRatingPanel.SetActive(false);
    }

    void Start()
    {
        bgMat = ratingBackground.material;
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
    }

    public void SetRating(GameObject starPanel, int rating, int index)
    {
        StartCoroutine(EaseInOutEffect(playerRatingPanel[index], 1.1768f, 1f, .5f));

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
            targetTransform.localScale = Vector3.Lerp( startScale * Vector3.one, endScale * Vector3.one, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = endScale * Vector3.one;
    }

    #region Network Receivers
    public void ReceiveNamesAndRatings(string[] playerNames, int[] starRating)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            int index = i;

            this.playerNames[index].text = playerNames[index];
            SetRating(starPanels[index], starRating[index], index);
        }
    }
    #endregion
}
