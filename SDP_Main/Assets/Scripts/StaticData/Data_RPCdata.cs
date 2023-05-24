using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[Serializable]
public struct s_GameScore
{
    public int numTeams;
    public int numPlayers;
    public int[] killsPerTeam;
    public int[] deathsPerTeam;
    public int[] killsPerPlayer;
    public int[] deathsPerPlayer;
}

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
[Serializable]
public struct s_DeathInfo
{
    public int diedId;
    public int diedTeam;
    public int killerId;
    public int killerTeam;
}
