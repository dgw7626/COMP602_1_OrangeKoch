using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MultiplayerEntity : MonoBehaviour
{
    Player_PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<Player_PlayerController>();
        if (Game_RuntimeData.isMultiplayer)
            Game_RuntimeData.activePlayers.Add(this);
    }
}
