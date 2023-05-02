using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_RuntimeData
{
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
    public static bool isMultiplayer = true;

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
    public static void DebugPrintMP_PlayerInfo()
    {
        Debug.Log("List of all instatiated players:");

        foreach(Player_MultiplayerEntity ent in instantiatedPlayers)
        {
            Debug.Log(ent.gameObject.name);
        }


        Debug.Log("List of active players players:");

        foreach(KeyValuePair<int, Player_MultiplayerEntity> ent in activePlayers)
        {
            Debug.Log("ID: " + ent.Value.GetComponent<PhotonView>().Owner.ActorNumber +
                " Name: " + ent.Value.uniqueID);
        }

    }
}
