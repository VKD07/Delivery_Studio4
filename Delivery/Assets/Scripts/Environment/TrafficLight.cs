using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] Texture2D stop, warning, go;
    [SerializeField] Renderer[] lightRenderers;
    bool isRedLight;
    #region RequiredComponents
    #endregion

    #region Getters
    public bool IsRedLight => isRedLight;
    #endregion

    private void Start()
    {
        SetLightsTexture(stop);
    }

    public void SetRedLight(bool value, float lightTimeInterval)
    {
        switch (value)
        {
            case true:
                StartCoroutine(SetTrafficStatus(stop, lightTimeInterval));
                isRedLight = true;
                break;

            case false:
                StartCoroutine(SetTrafficStatus(go, lightTimeInterval));
                isRedLight = false;
                break;
        }
    }

    IEnumerator SetTrafficStatus(Texture2D textureName, float timeInterval)
    {
        SetLightsTexture(warning);
        yield return new WaitForSeconds(timeInterval);
        SetLightsTexture(textureName);
    }

    void SetLightsTexture(Texture2D textureName)
    {
        foreach (Renderer r in lightRenderers)
        {
            Material mat = r.material;
            mat.SetTexture("_EmissionMap", textureName);
        }
    }
}
