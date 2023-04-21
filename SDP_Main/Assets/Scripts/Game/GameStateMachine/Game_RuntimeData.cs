using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_RuntimeData
{
    public static List<Player_MultiplayerEntity> instantiatedPlayers = new List<Player_MultiplayerEntity>();
    public static List<List<Player_MultiplayerEntity>> teams = new List<List<Player_MultiplayerEntity>>();
    public static Dictionary<int, Player_MultiplayerEntity> activePlayers = new Dictionary<int, Player_MultiplayerEntity>(); 
    public static bool isMultiplayer = true;
    public static IgameMode gameMode = null;
    public static List<string> GameMap_List;

    public static void DebugPrintMP_PlayerInfo()
    {
        Debug.Log("List of instatiated players:");
        
        foreach(Player_MultiplayerEntity ent in instantiatedPlayers) 
        {
            Debug.Log(ent.gameObject.name);
        }


        Debug.Log("List of instatiated players:");
        
        foreach(KeyValuePair<int, Player_MultiplayerEntity> ent in activePlayers)
        {
            Debug.Log("ID: " + ent.Value.GetComponent<PhotonView>().Owner.ActorNumber + 
                " Name: " + ent.Value.uniqueID);
        }

    }
}
