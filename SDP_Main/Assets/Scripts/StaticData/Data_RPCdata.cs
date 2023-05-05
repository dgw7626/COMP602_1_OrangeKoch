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
    public int dmgDeltId;
    public float dmgValue;
    public e_BodyPart bodyPart;
}

public class Data_RPCdata : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
