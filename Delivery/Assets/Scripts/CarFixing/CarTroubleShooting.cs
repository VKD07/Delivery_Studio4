using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarTroubleshoot", menuName = "CarMalfunction/Create new problem")]
public class CarTroubleShooting : ScriptableObject
{
    [SerializeField] KeyCode[] keysToPressforFix;
    [SerializeField] public float timeToHold;
    [SerializeField] Color colorOfSmoke;
    [SerializeField] Sprite symbolToShow;

    public Color GetSmokeColor()
    {
        return colorOfSmoke;
    }

    public Sprite getSpriteSymbol()
    {
        return symbolToShow;
    }

    public bool CheckifCarIsFixed()
    {
        bool isFixed = true;

        for (int i = 0; i < keysToPressforFix.Length; i++)
        {
            if (!Input.GetKey(keysToPressforFix[i]))
            {
                isFixed = false;
                break;
            }
        }
        return isFixed;
    }
}
