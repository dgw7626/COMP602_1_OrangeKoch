using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Author: Corey Knight - 21130891
 */
public class Game_RuntimeData
{
    /// <summary>
    /// Reference to game mode manager to be accessed from instantiated environment.
    /// </summary>
    public static GameMode_Manager gameMode_Manager = null;

    /// <summary>
    /// List of all Multiplayer Entities that were ever instantiated.
    /// Persists even if a player leaves a match.
    /// </summary>
    public static List<Player_MultiplayerEntity> instantiatedPlayers = new List<Player_MultiplayerEntity>();

    /// <summary>
    /// List of team lists of Multiplayer Entities
    /// </summary>
    public static List<List<Player_MultiplayerEntity>> teams = new List<List<Player_MultiplayerEntity>>();

    /// <summary>
    /// Key-Value pairs of Unique ID's and active Multiplayer Entities.
    /// </summary>
    public static Dictionary<int, Player_MultiplayerEntity> activePlayers = new Dictionary<int, Player_MultiplayerEntity>();

    /// <summary>
    /// This can prevent many multiplayer-specific action during a single player match
    /// </summary>
    public static bool isMultiplayer = false;

    /// <summary>
    /// Global state for the multiplayer game
    /// </summary>
    public static bool matchIsRunning = false;

    /// <summary>
    /// Reference to the local player client to store their configuration settings
    /// </summary>
    public static Player_Settings playerSettings = new Player_Settings();

    /// <summary>
    /// Will be set to Standard if not other type has been selected from the menu.
    /// </summary>
    public static IgameMode gameMode = null;

    /// <summary>
    /// A list set to store all the GameMaps that can be used to change scene
    /// </summary>
    public static List<string> GameMap_List;
    /// <summary>
    /// The Photon View belonging to the local player
    /// </summary>
    public static PhotonView thisMachinesPlayersPhotonView = null;
    /// <summary>
    /// The Photon View belonging to the local player
    /// </summary>
    public static Player_MultiplayerEntity thisMachinesMultiplayerEntity = null;

    /// <summary>
    /// Informattion about the game score. Will be updated only by the master client, who
    /// will broadcast the struct at the end of a match.
    /// </summary>
    public static s_GameScore gameScore;

    /// <summary>
    /// Key-value-pair of all currently active players in a multiplayer match.
    /// Triggered once at the start of a match, by the GameModeManager. All MultiplayerEntities in instantiatedPlayers will be added.
    ///
    /// Triggered again by the Pun RPC callback OnPlayerEnteredRoom, which is handled by the specific game mode.
    /// This allows players who joined after the start of a match to be listed.
    /// </summary>
    ///
    /// <param name="id">A unique ID assigend to a player by Photon.</param>
    ///
    /// <param name="entity">The players Player_MultiplayerEnitity</param>
    public static void RegisterNewMultiplayerPlayer(int id, Player_MultiplayerEntity entity)
    {
        if (activePlayers.ContainsKey(id))
        {
            Debug.LogError("FATAL UNHANDLED ERROR!\nCANNOT ADD NEW PLAYER! PLAYER ID: " + id + " IS ALREADY REGISTERED");
            return;
        }

        activePlayers.Add(id, entity);
        entity.RegisterUniqueID("" + id);
        Debug.Log("Registered new player: " + id);
    }


    /// <summary>
    /// Cleanup and destroy objects when exiting Multiplayer Game
    /// </summary>
    public static void CleanUp_Multiplayer_Data()
    {
        Debug.Log("Quit Multiplayer Invoked - Returning to Main Multiplayer_MenuItem.");
        if (Game_RuntimeData.isMultiplayer)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        //TODO: these members need to be alive after this method, because
        // they are required to cleanup the game.
        // consider another cleanup method to garbage collect these after the scoreboard.
        //gameMode_Manager = null;
        instantiatedPlayers = new List<Player_MultiplayerEntity>();
        teams = new List<List<Player_MultiplayerEntity>>();
        activePlayers = null;
        isMultiplayer = false;
        //gameMode = null;
        thisMachinesPlayersPhotonView = null;
    }
}
