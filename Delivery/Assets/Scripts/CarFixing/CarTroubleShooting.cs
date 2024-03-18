using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "CarTroubleshoot", menuName = "CarMalfunction/Create new problem")]
public class CarTroubleShooting : ScriptableObject
{
    [SerializeField] KeyCode[] keysToPressforFix;
    [SerializeField] public float timeToHold;
    [SerializeField] Color colorOfSmoke;
    [SerializeField] Sprite symbolToShow;
    [SerializeField, TextArea] string instructionsTxt;

    public Color GetSmokeColor => colorOfSmoke;
    public Sprite getSpriteSymbol => symbolToShow;
    public int numOfKeysToPress => keysToPressforFix.Length;
    public string GetInstructionsTxt => instructionsTxt;

    public bool CheckifCarIsFixed()
    {
        bool isFixed = true;

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (!keysToPressforFix.Contains(keyCode) && Input.GetKey(keyCode))
            {
                isFixed = false;
                break;
            }
        }

        foreach (KeyCode key in keysToPressforFix)
        {
            if (!Input.GetKey(key))
            {
                isFixed = false;
                break;
            }
        }

        return isFixed;
    }
}