using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct DamageStruct
{
    public Player_MultiplayerEntity damageDealer;
    public Player_MultiplayerEntity damageReciever;

    public float damageTotal;
}

public struct SoundStruct
{
    public Transform location;
    public AudioClip sound;
}
