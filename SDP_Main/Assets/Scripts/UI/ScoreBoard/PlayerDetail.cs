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

/// <summary>
/// The constructor assign player detail with scoreboard variables.
/// </summary>
    public PlayerDetail()
    {
        playerName = ScoreBoard.playerName;
        kills = ScoreBoard.playerKills;
        deaths = ScoreBoard.playerDeaths;
    }
}
