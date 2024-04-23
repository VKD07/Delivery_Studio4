using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "SetOfMap", menuName ="Navigator/Create new set of maps")]
public class NavigatorMapCreator : ScriptableObject
{
    public GameObject[] maps;
    public string sceneName;
}
