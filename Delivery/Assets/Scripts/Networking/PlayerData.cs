using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string name { get; set; }
    public int teamNumber { get; set; }
    
    public GameRole role { get; set; }

    public PlayerData(string name, int teamNumber, GameRole role)
    {
        this.name = name;
        this.teamNumber = teamNumber;
        this.role = role;
    }
}

[Serializable]
public enum GameRole
{
    None = 0,
    Driver,
    Navigator
}
