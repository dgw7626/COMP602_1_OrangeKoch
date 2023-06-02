using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Corey Knight - 21130891
 */
public class Game_MultiplayerState : AbstractState
{
    public IgameMode gameMode;
    public override AbstractState RunState(object param)
    {
        // Zero out multiplayer RuntimeData
        Game_RuntimeData.instantiatedPlayers.Clear();
        Game_RuntimeData.teams.Clear();
        Game_RuntimeData.isMultiplayer = true;

        // Set GameMode to default. May be overriden
        gameMode = new GameMode_Standard();

        return this;
    }
}