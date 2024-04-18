using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string name { get; set; }
    public int teamNumber { get; set; }

    public GameRole role { get; set; }

    public LobbyMode mode { get; set; }

    public int winner { get; set; }
    public string time {  get; set; }

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

[Serializable]
public enum LobbyMode
{
    None = 0,
    Duo,
    TwoVTwo,
    WarmUp
}
