/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Munish Kumar                *
 * Student ID: 		19083476		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class ScoreManager
{
    public string playerName;
    public int kills;
    public int deaths;
    public int winningTeamName;
    public int team_1TotalKills;

    /// <summary>
    /// The constructor takes player details and assign to the variables.
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="kills"></param>
    /// <param name="deaths"></param>
    public ScoreManager(string playerName, int kills, int deaths)
    {
        this.playerName = playerName;
        this.kills = kills;
        this.deaths = deaths;
    }
    /// <summary>
    /// The constructor take winning team name and assign it. 
    /// </summary>
    /// <param name="winningTeamName"></param>
    public ScoreManager(int winningTeamName)
    {
        this.winningTeamName = winningTeamName;
    }

}
