using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;

public class ScoreManager
{
    public string playerName;
    public int kills;
    public int deaths;
    public int won;

// The constructor takes player details and put data on Firebase Database.
    public ScoreManager(string playerName, int kills, int deaths, int won)
    {
        this.playerName = playerName;
        this.kills = kills;
        this.deaths = deaths;
        //this.won = won;
    }

    
}
