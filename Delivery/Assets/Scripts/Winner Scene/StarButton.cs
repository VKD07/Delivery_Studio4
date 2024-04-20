using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] int starNumber;

    public void OnPointerEnter(PointerEventData eventData)
    {
        RateYourPartnerManager.instance.currentRating = starNumber;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        RateYourPartnerManager.instance.SendRating();
    }
}
