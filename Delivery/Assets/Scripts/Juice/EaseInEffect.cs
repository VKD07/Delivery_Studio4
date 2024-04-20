using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseInEffect : MonoBehaviour
{
    [SerializeField] float initSize;
    [SerializeField] float targetSize;
    [SerializeField] float scaleSpeed = 10f;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * initSize;

        StartCoroutine(ScaleToTargetSize());
    }

    IEnumerator ScaleToTargetSize()
    {
        Vector3 initScale = transform.localScale;
        Vector3 targetScale = Vector3.one * targetSize;

        float distance = Vector3.Distance(initScale, targetScale);

        float duration = distance / scaleSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            transform.localScale = Vector3.Lerp(initScale, targetScale, t);

            yield return null;
        }
    }
}
