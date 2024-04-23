using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Driver
{
    [CreateAssetMenu(fileName = "DriverControls", menuName = "Driver/Controls")]
    public class DriverControls : ScriptableObject
    {
        public Inputs horizontal;
        public Inputs vertical;
        public KeyCode breakKey;
        public KeyCode carHorn;
        public KeyCode repositionKey = KeyCode.I;
    }

    #region Enum
    [Serializable]
    public enum Inputs
    {
        Vertical,
        Horizontal
    }
    #endregion
}