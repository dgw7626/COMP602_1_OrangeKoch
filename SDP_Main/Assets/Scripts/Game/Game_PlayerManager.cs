using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_PlayerManager : MonoBehaviourPunCallbacks
{
    public Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Create a new player game object and add it to the playerObjects dictionary
        GameObject playerObject = new GameObject("Player_" + newPlayer.ActorNumber);
        playerObjects.Add(newPlayer.ActorNumber, playerObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Remove the player game object from the playerObjects dictionary
        GameObject playerObject = playerObjects[otherPlayer.ActorNumber];
        playerObjects.Remove(otherPlayer.ActorNumber);
        Destroy(playerObject);
    }

    public GameObject GetPlayerObject(int playerId)
    {
        // Return the player game object with the specified ID
        return playerObjects[playerId];
    }

    private void Awake()
    {
        Game_RuntimeData.playerManager = this;
    }

    private void OnDestroy()
    {
        Game_RuntimeData.playerManager = null;
    }
}
