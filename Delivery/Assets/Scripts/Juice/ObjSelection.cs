using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjSelection : MonoBehaviour, MouseSelection
{
    [SerializeField] Effect[] objEffect;

    [Header("=== RESIZE OBJECT ===")]
    [SerializeField] float maxSize;
    [SerializeField] float resizeSpeed = 3f;
    bool resizeObj;
    Vector3 initObjSize;

    [Space(4)]
    [Header("On Hover Event")]
    [SerializeField] UnityEvent OnHover;
    [SerializeField] UnityEvent UnHover;

    [Header("On Click Event")]
    [SerializeField] UnityEvent OnClick;

    private void Awake()
    {
        initObjSize = transform.localScale;
    }
    private void Update()
    {
        ResizeObj();
    }

    private void ResizeObj()
    {
        if (resizeObj)
        {
            OnHover.Invoke();
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * maxSize, resizeSpeed * Time.deltaTime);
        }
        else
        {
            UnHover.Invoke();
            transform.localScale = Vector3.Lerp(transform.localScale, initObjSize, resizeSpeed * Time.deltaTime);
        }
        resizeObj = false;
    }


    void ResizeClickEffect()
    {
        resizeObj = false;
        transform.localScale = initObjSize;

        while (Vector3.Distance(transform.localScale, Vector3.one * maxSize) < .1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * maxSize, resizeSpeed * Time.deltaTime);
        }
    }


    public void TriggerHoverEffect()
    {
        resizeObj = true;
    }


    public void TriggerClickEffect()
    {
        OnClick.Invoke();


        ResizeClickEffect();
    }
}

public enum Effect
{
    None = 0,
    Resize,
}
