using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudSplashManager : MonoBehaviour
{
    public static MudSplashManager instance;

    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Sprite[] mudSprites;
    [SerializeField] float splashSpeed = .1f;
    [SerializeField] ParticleSystem mudFx;

    [Header("=== DISSOLVE SETTINGS ===")]
    [SerializeField] float dissolveDelay = 5f;
    [SerializeField] float dissolveSpeed = .1f;
    [SerializeField] Material mudSplashMat;

    bool isEnabled;
    float targetScale = 0.09004044f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this);
        }
    }

    public void EnableMudSplash()
    {
        if(!isEnabled)
        {
            isEnabled = true;
            StartCoroutine(ScaleSprites());
            //NetworkSender.instance?.SendDirtPacket();
            SendPackets.SendDirtCollision();
        }
    }

    private IEnumerator ScaleSprites()
    {
        mudSplashMat.SetFloat("_DissolveAmount", 1);
        mudFx.Play();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].transform.localScale = Vector3.zero;
            spriteRenderers[i].enabled = true;
            int randomImage = Random.Range(0, mudSprites.Length);
            spriteRenderers[i].sprite = mudSprites[randomImage];

            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                spriteRenderers[i].transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(targetScale, targetScale, targetScale), elapsedTime);
                elapsedTime += Time.deltaTime * splashSpeed;
                yield return null;
            }
            spriteRenderers[i].transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        }
    }

    public void WipeOffMud()
    {
        StartCoroutine(WipeOffMudCouroutine());
    }

    IEnumerator WipeOffMudCouroutine()
    {
        if(isEnabled)
        {
            float dissolveTime = mudSplashMat.GetFloat("_DissolveAmount");
            while (dissolveTime > 0f)
            {
                dissolveTime -= Time.deltaTime * dissolveSpeed;
                mudSplashMat.SetFloat("_DissolveAmount", dissolveTime);
                yield return null;
            }
            isEnabled = false;
        }
    }
}