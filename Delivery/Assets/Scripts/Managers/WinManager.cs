using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager instance;  

    [SerializeField] GameObject WinPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void DeclareWinner(bool value)
    {
        if (!value) return;
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
