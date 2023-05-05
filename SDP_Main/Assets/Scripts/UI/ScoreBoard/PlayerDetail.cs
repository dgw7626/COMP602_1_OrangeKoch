using System.IO;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class PlayerDetail 
{
    public string playerName;
    public int kills;
    public int deaths;
    //public int won;

// The constructor assign player detail with scoreboard features.
    public PlayerDetail()
    {
        playerName = ScoreBoard.playerName;
        kills = ScoreBoard.playerKills;
        deaths = ScoreBoard.playerDeaths;
        //won = ScoreBoard.playerWon;
    }
}
