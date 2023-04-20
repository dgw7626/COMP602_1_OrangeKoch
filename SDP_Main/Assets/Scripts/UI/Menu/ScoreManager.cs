using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;

//[Serializable]

public class ScoreManager
{
    public string playerName;
    public int kills;
    public int deaths;
    public int won;

    public ScoreManager(string playerName, int kills, int deaths, int won)
    {
        this.playerName = playerName;
        this.kills = kills;
        this.deaths = deaths;
        this.won = won;
    }

    
}
