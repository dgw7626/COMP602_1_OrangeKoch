using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Standard : IgameMode
{
    public void InitGame()
    {
        // Make two teams
        Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
        Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());

        //Divy players up into teams
        for(int i = 0; i < Game_RuntimeData.activePlayers.Count; i++)
        {
            int team = i % 2 == 0 ? 0 : 1;

            Game_RuntimeData.teams[team].Add(Game_RuntimeData.activePlayers[i]);
        }
        StartGame();
    }

    public void StartGame()
    {
        // Unlock Player Movement
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.activePlayers)
        {
            p.playerController.IsMultiplayer = true;
            p.playerController.IsInputLocked = false;
        }
    }

    public void StopGame()
    {
        //TODO: Cleanup
    }

}
