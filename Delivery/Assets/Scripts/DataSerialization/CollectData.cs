using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectData : MonoBehaviour
{
    public static CollectData instance;

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

    //Create all the variables of data here --------------------------
    //Make sure it is public
    public int kioMapCrashCount;
    public int cityMapCrashCount;
    public string averagePlayTime;
    public int playerCount;
    public string carColor = "DefaultTex";
    public string mapSkin = "DEFAULT";
}

//    Data to collect:
//- Car color picked
//- nav map Wall paper picked
//- Average playtime 
//- Average player count
//- Average Car crash count
// If app is close collect data