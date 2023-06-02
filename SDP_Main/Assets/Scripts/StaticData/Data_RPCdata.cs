/*

 ************************************************
 *                                              *
 * Primary Dev: 	Corey Knigth	            *
 * Student ID: 		21130891		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *
 ************************************************

*/
using System;

/// <summary>
/// Used to identify body parts on the player model
/// </summary>
[Serializable]
public enum e_BodyPart
{
    NONE,
    HEAD,
    FACE,
    BODY,
    ARMS,
    LEGS
}

/// <summary>
/// Used to pass game scores between calsses
/// </summary>
[Serializable]
public struct s_GameScore
{
    public int numTeams;
    public int numPlayers;
    public int[] killsPerTeam;
    public int[] deathsPerTeam;
    public int[] killsPerPlayer;
    public int[] deathsPerPlayer;
    public int[] teamNumbersByPlayer;
}

/// <summary>
/// Used to pass damage across classes
/// </summary>
[Serializable]
public struct s_DamageInfo
{
    public int dmgDealerId;
    public int dmgRecievedId;
    public int dmgDealerTeam;
    public int dmgRecievedTeam;
    public float dmgValue;
    public e_BodyPart bodyPart;
}

/// <summary>
/// Used to pass death events across classes
/// </summary>
[Serializable]
public struct s_DeathInfo
{
    public int diedId;
    public int diedTeam;
    public int killerId;
    public int killerTeam;
}
