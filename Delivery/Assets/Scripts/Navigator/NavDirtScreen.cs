using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavDirtScreen : MonoBehaviour
{
    public static NavDirtScreen instance;

    [SerializeField] SpriteRenderer[] dirtImageUI;
    [SerializeField] Sprite[] mudSprites;
    [SerializeField] float splashSpeed = .1f;

    [Header("=== DISSOLVE SETTINGS ===")]
    [SerializeField] float dissolveDelay = 5f;
    [SerializeField] float dissolveSpeed = .1f;
    [SerializeField] Material mudSplashMat;

    float targetScale = 14f;

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
        SetAllSpritesScaleToZero();
    }
    public void EnableMudSplash()
    {
        StartCoroutine(ScaleSprites());
    }

    void SetAllSpritesScaleToZero()
    {
        for (int i = 0; i < dirtImageUI.Length; i++)
        {
            dirtImageUI[i].transform.localScale = Vector3.zero;
        }
    }

    private IEnumerator ScaleSprites()
    {
        mudSplashMat.SetFloat("_DissolveAmount", 1);
        for (int i = 0; i < dirtImageUI.Length; i++)
        {
            dirtImageUI[i].transform.localScale = Vector3.zero;
            dirtImageUI[i].enabled = true;
            int randomImage = Random.Range(0, mudSprites.Length);
            dirtImageUI[i].sprite = mudSprites[randomImage];

            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                dirtImageUI[i].transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(targetScale, targetScale, targetScale), elapsedTime);
                elapsedTime += Time.deltaTime * splashSpeed;
                yield return null;
            }
            dirtImageUI[i].transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        }
    }

    public void ReceiveDriverWipers()
    {
        StartCoroutine(WiperCoroutine());
    }
    IEnumerator WiperCoroutine()
    {
        float dissolveTime = mudSplashMat.GetFloat("_DissolveAmount");
        while (dissolveTime > 0f)
        {
            dissolveTime -= Time.deltaTime * dissolveSpeed;
            mudSplashMat.SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
    }

}
