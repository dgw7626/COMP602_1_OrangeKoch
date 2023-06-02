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

/// <summary>
/// Used by the GameStateMachine to execute state specific logic in multiplayer mode
/// </summary>
public class Game_MultiplayerState : AbstractState
{
    public IgameMode gameMode;

    /// <summary>
    /// Run the state.
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
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