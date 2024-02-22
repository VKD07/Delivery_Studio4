using Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NavigatorControls", menuName = "Navigator/Controls")]

public class NavigatorControls : ScriptableObject
{
    [Header("=== MAP ROTATION KEYS ===")]
    public KeyCode xRotationKey;
    public KeyCode yRotationKey;

    [Header("=== MAP RESIZE ===")]
    public ResizeMode resizeMode;
}

[Serializable]
public enum ResizeMode{
    Keyboard,
    ScrollWheel
}
