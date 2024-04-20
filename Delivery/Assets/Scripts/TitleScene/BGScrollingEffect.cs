using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BGScrollingEffect : MonoBehaviour
{
    [SerializeField] Image ratingBackground;
    Material bgMat;
    public float scrollSpeed = 0.5f;

    private void Start()
    {
        bgMat = ratingBackground.material;
    }

    void Update()
    {
        ScrollingEffect();
    }

    private void ScrollingEffect()
    {
        float offset = Time.time * scrollSpeed;
        bgMat.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }
}
