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

    public ScoreManager()
    {
        playerName = PutPlayerDetailOnFirebase.playerName;
        kills = PutPlayerDetailOnFirebase.playerKills;
        deaths = PutPlayerDetailOnFirebase.playerDeaths;
        won = PutPlayerDetailOnFirebase.playerWon;
    }
}
