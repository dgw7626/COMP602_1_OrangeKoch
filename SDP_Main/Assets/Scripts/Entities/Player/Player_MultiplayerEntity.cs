using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Corey Knight - 21130891
 */

/// <summary>
/// Represents an active player. Contains PunRPC callbacks for all multiplayer network actions.
/// This class is attatched to the Player prefab, so will be present even in singleplayer.
///
/// </summary>
public class Player_MultiplayerEntity : MonoBehaviour
{
    // The controller that is attatched to this PlayerPrefab
    public Player_PlayerController playerController;
    // A unique identifier for multiplayer matches
    public string uniqueID {  get; private set; }
    public int teamNumber;

    //--------------------------------------------------------------------------------------------------------------

    //TODO: May be redundant. A Unique ID is stored as a key-value pair in Game_RuntimeData upon entering a match.
    // May be used in future for account sign-in.
    public bool RegisterUniqueID(string uniqueID)
    {
        if (this.uniqueID != null)
            return false;

        this.uniqueID = uniqueID;
        return true;
    }

 /*   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(uniqueID);
        }
    }*/

    private void Start()
    {
        // Finde the PlayerController within this game object
        playerController = GetComponent<Player_PlayerController>();

        if (Game_RuntimeData.isMultiplayer)
        {
            // Lock input. The GameMode will unlock input when it is neccecary.
            playerController.IsInputLocked = true;

            // Add to list of all Multiplayers that were ever instantiated.
            // May be used for statistics.
            Game_RuntimeData.instantiatedPlayers.Add(this);

            // Register the PhotnView with the local machine
            if(playerController.photonView.IsMine)
            {
                Game_RuntimeData.thisMachinesPlayersPhotonView = playerController.photonView;
            }
        }
    }

    /// <summary>
    /// PunRPC callback to register damage taken within a player.
    /// The damage dealer will track how much damage they have delt.
    /// It is up to the damage reciever to decide whether they should die. If so, they must broadcast
    /// a message to all other players to inform them.
    ///
    /// </summary>
    /// The ammount of damage recieved. This is decided by the calller.
    /// <param name="damage"></param>
    [PunRPC]
    public void OnDamageRecieved(float damage)
    {
        /*if (!playerController.photonView.IsMine)
        {
            Debug.Log("I am Not executing because it is not me");
            return;
        }*/
        Debug.Log("I am executing because it is me!");
        //TODO: Calculate Damage
    }

    /// <summary>
    /// Gets the sychronous clock time from the Master Client.
    /// The Master Client triggers this callback on all other active players, once every second.
    ///
    /// </summary>
    /// The synchronous clock time
    /// <param name="value"></param>

    [PunRPC]
    public void GetSynchronousTimerValue(int value)
    {
        GameMode_Manager.gameTime = value;
    }
}
