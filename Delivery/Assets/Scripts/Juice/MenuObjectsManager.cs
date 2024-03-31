using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectsManager : MonoBehaviour
{
    [SerializeField] GameObject[] mainMenuObjs;
    private void Awake()
    {
        SetActiveSelectionEffect(false);
    }
    public void SetActiveSelectionEffect(bool val)
    {
        for (int i = 0; i < mainMenuObjs.Length; i++)
        {
            ObjSelection selection = mainMenuObjs[i].GetComponent<ObjSelection>();
            selection.enabled = val;
        }
    }
}
