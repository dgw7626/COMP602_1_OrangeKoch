using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Author: Corey Knight - 21130891
 */

/// <summary>
/// Represents an active player. Contains PunRPC callbacks for all multiplayer network actions.
/// This class is attatched to the Player prefab, so will be present even in singleplayer.
///
/// </summary>
public class Player_MultiplayerEntity : MonoBehaviourPunCallbacks
{
    // The controller that is attatched to this PlayerPrefab
    public Player_PlayerController playerController;

    public Player_Health playerHealth;

    // A unique identifier for multiplayer matches
    public string uniqueID { get; private set; }
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

    private void Start()
    {
        // Finde the PlayerController within this game object
        playerController = GetComponent<Player_PlayerController>();


        playerHealth = gameObject.GetComponent<Player_Health>();

        if (Game_RuntimeData.isMultiplayer)
        {
            // Lock input. The GameMode will unlock input when it is neccecary.
            playerController.IsInputLocked = true;

            // Add to list of all Multiplayers that were ever instantiated.
            // May be used for statistics.
            Game_RuntimeData.instantiatedPlayers.Add(this);

            // Register the PhotnView with the local machine
            if (playerController.photonView.IsMine)
            {
                Game_RuntimeData.thisMachinesPlayersPhotonView = playerController.photonView;
                Game_RuntimeData.thisMachinesMultiplayerEntity = this;
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
    public void OnDamageRecieved(string damageInfo)
    {
        s_DamageInfo dmgInfo = (s_DamageInfo)JsonUtility.FromJson(damageInfo, typeof(s_DamageInfo));
        if (PhotonNetwork.LocalPlayer.ActorNumber != dmgInfo.dmgRecievedId)
            return;

        //Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(dmgInfo.dmgRecievedId);
        Debug.Log("SHOULD ONLY BE PLAYER: I am the player: " + playerController.photonView.Owner.ActorNumber + "\nBut I should be: " + PhotonNetwork.LocalPlayer.ActorNumber);
        if (playerController.photonView.IsMine)
        {
            Debug.Log("The PV is mine (" + dmgInfo.dmgRecievedId + ")and I was shot by " + dmgInfo.dmgDealerId);
        }

        if (playerController.photonView.IsMine)
        {
            Debug.Log("My actor number: " + (playerController.photonView.IsMine ? PhotonNetwork.LocalPlayer.ActorNumber : playerController.photonView.Owner.ActorNumber));
            Debug.Log("The actor who was shot: " + dmgInfo.dmgRecievedId);
            playerHealth.TakeDamage(dmgInfo);

        }
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

    /// <summary>
    /// PunRPC callback. You are being informed that another player was killed in the match.
    /// The player that was killed will broadcast this message to all other players.
    /// </summary>
    /// <param name="killerId"></param>
    /// <param name="killedTeamNumber"></param>
    [PunRPC]
    public void OnPlayerKilled(string deathInfoStructJSON)
    {
        s_DeathInfo info = (s_DeathInfo)JsonUtility.FromJson(deathInfoStructJSON, typeof(s_DeathInfo));
        if (PhotonNetwork.IsMasterClient)
        {
            Game_RuntimeData.gameMode.OnScoreEvent(info);
        }
        if (Game_RuntimeData.thisMachinesPlayersPhotonView.IsMine)
        {
            Game_RuntimeData.gameMode.OnPlayerKilled(info);
        }
    }

    /// <summary>
    /// PunRPC callback. The master client has announced that the game has ended, and is telling you the score.
    /// </summary>
    [PunRPC]
    public void UpdateScore(string gameScoreStructJSON)
    {
        s_GameScore gameScoreStruct = (s_GameScore)JsonUtility.FromJson(gameScoreStructJSON, typeof(s_GameScore));

        //TODO: store the data into DB?

        // MunishesScoreStuff.HereIsTheScore(gameScoreStruct);
        Game_RuntimeData.gameScore = gameScoreStruct;
    }

    /// <summary>
    /// Someone is requesting a score update. If you are the master client, broadcast back the current score
    /// </summary>
    [PunRPC]
    public void RequestScoreFromMaster()
    {
        if(PhotonNetwork.IsMasterClient) 
        {
            photonView.RPC(nameof(UpdateScore), RpcTarget.All, JsonUtility.ToJson(Game_RuntimeData.gameScore));
        }
    }

    /// <summary>
    /// PunRPC callback. making the player character invincible
    /// </summary>
    [PunRPC]
    public void OnRespawn()
    {
        playerHealth.isInvincible = true;
        Invoke(nameof(TurnOffInvincibility), 5.0f);
    }
    
    /// <summary>
    /// turning off the invincibility 
    /// </summary>
    private void TurnOffInvincibility()
    {
        playerHealth.isInvincible = false;
    }
}
