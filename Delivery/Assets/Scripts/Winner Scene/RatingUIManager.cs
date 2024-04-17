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

    [Header("=== TEAM 1 UI ===")]
    [SerializeField] TextMeshProUGUI[] playerNames;
    [SerializeField] GameObject[] starPanels;

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

    public void SetRating(GameObject starPanel, int rating)
    {
        for (int i = 0; i < rating; i++)
        {
            starPanel.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
        }
    }

    #region Network Receivers
    public void ReceiveNamesAndRatings(string[] playerNames, int[] starRating)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            int index = i;

            this.playerNames[index].text = playerNames[index];
            SetRating(starPanels[index], starRating[index]);
        }
    }
    #endregion
}
