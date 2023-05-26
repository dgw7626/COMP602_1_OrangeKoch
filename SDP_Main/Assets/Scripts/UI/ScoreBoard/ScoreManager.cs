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
    public int winningTeamName;
    public int team_1TotalKills;

// The constructor takes player details and put data on Firebase Database.
    public ScoreManager(string playerName, int kills, int deaths)
    {
        this.playerName = playerName;
        this.kills = kills;
        this.deaths = deaths;
    }

    public ScoreManager(int winningTeamName)
    {
        this.winningTeamName = winningTeamName;
        
    }

    public ScoreManager(int team_1TotalKills)
    {
        this.team_1TotalKills = team_1TotalKills;
    }


}
