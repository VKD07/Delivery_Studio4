using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string name { get; private set; }
    public string partnerName { get; private set; }
    
    public GameRole role { get; private set; }

    public PlayerData(string name, string partnerName, GameRole role)
    {
        this.name = name;
        this.partnerName = partnerName;
        this.role = role;
    }
}

[Serializable]
public enum GameRole
{
    Driver,
    Navigator
}
