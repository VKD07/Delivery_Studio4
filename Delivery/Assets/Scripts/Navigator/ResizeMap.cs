using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MapActivator))]
public class ResizeMap : MonoBehaviour
{
    [SerializeField] NavigatorControls navControls;
    [SerializeField] Transform mapsParent;

    [Header("=== RESIZE SETTING ===")]
    [Range(0, 1)]
    [SerializeField] float resizeStrength = 1f;
    [Range(1, 5)]
    [SerializeField] float mapMaxScaleLimit = 1.6f;
    [Range(.5f, 1)]
    [SerializeField] float mapMinScaleLimit = .5f;

    private void Update()
    {
        AdjustMapScale();
    }

    private void AdjustMapScale()
    {
        if (mapsParent == null) { return; }
        switch (navControls.resizeMode)
        {
            case ResizeMode.ScrollWheel:
                ResizeMapUsingScrollWheel();
                break;
            case ResizeMode.Keyboard:
                ResizeMapUsingKeyboard();
                break;
        }
    }

    void ResizeMapUsingScrollWheel()
    {
        if ((mapsParent.localScale.y < mapMinScaleLimit && Input.mouseScrollDelta.y < 0)
            || (mapsParent.localScale.y >= mapMaxScaleLimit && Input.mouseScrollDelta.y > 0))
        {
            return;
        }

        if ((mapsParent.localScale.y >= mapMinScaleLimit || Input.mouseScrollDelta.y > 0) &&
            (mapsParent.localScale.y <= mapMaxScaleLimit || Input.mouseScrollDelta.y < 0))
        {
            mapsParent.localScale += Vector3.one * resizeStrength * Input.mouseScrollDelta.y;
        }
    }

    void ResizeMapUsingKeyboard()
    {
        if ((mapsParent.localScale.y <= mapMinScaleLimit && Input.GetKeyDown(KeyCode.S)) ||
            (mapsParent.localScale.y >= mapMaxScaleLimit && Input.GetKeyDown(KeyCode.W)))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && mapsParent.localScale.y < mapMaxScaleLimit)
        {
            mapsParent.localScale += Vector3.one * resizeStrength * 1;
        }

        if (Input.GetKeyDown(KeyCode.S) && mapsParent.localScale.y > mapMinScaleLimit)
        {
            mapsParent.localScale -= Vector3.one * resizeStrength * 1;
        }
    }
}
