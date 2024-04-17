using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RateYourPartnerManager : MonoBehaviour
{
    public static RateYourPartnerManager instance;

    [SerializeField] Button[] stars;
    [SerializeField] public int currentRating { get; set; }
    bool hasRated;

    RatingUIManager ratingUIManager => GetComponent<RatingUIManager>();
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

    private void Update()
    {
        LightUpStars();
    }

    private void LightUpStars()
    {
        if (hasRated) return;
        DisableAllLights();
        for (int i = 0; i < currentRating; i++)
        {
            stars[i].image.color = Color.yellow;
        }
    }

    void DisableAllLights()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].image.color = Color.gray;
        }
    }

    public void SendRating()
    {
        if (!hasRated)
        {
            ratingUIManager.EnableOverAllRating();
            hasRated = true;
            SendPackets.SendRatingToPartner(currentRating);
        }
        //TODO: Send Rating Packet to your partner
    }
}
