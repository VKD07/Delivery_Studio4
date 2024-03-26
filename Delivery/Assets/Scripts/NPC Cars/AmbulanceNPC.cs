using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceNPC : MonoBehaviour
{
    [SerializeField] float lightInterval = .5f;
    [SerializeField] Texture2D[] ambulanceEmissiveTex;
    Material mat;
    Renderer renderer => GetComponent<Renderer>();

    private void Awake()
    {
        mat = renderer.material;
    }
    private void OnEnable()
    {
        StartCoroutine(LightLoop());
    }

    IEnumerator LightLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(lightInterval);
            mat.SetTexture("_EmissionMap", ambulanceEmissiveTex[0]);
            yield return new WaitForSeconds(lightInterval);
            mat.SetTexture("_EmissionMap", ambulanceEmissiveTex[1]);
        }
    }
}
