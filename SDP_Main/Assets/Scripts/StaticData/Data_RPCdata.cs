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
    public List<int> killsPerTeam;
}

[Serializable]
public struct s_DamageInfo
{
    public int dmgDealerId;
    public int dmgRecievedId;
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
