using Photon.Pun;
using Photon.Realtime;
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
            if(playerController.photonView.IsMine)
            {
                Game_RuntimeData.thisMachinesPlayersPhotonView = playerController.photonView;
                Game_RuntimeData.thisMachinesMultiplayerEntity = this;
            }
        }
    }

    public void DamagePlayer(int idOfShotPlayer)
    {
        Debug.Log("I " + playerController.photonView.Owner.ActorNumber + " am shooting: " + idOfShotPlayer);

        s_DamageInfo dmg = new s_DamageInfo();
        dmg.dmgDeltId = idOfShotPlayer;
        //dmg.dmgDealerId = Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber;
        dmg.dmgDealerId = playerController.photonView.Owner.ActorNumber;
        dmg.bodyPart = e_BodyPart.NONE;

        PhotonView pv = null;
        foreach (KeyValuePair<int, Player_MultiplayerEntity> kv in Game_RuntimeData.activePlayers)
        {
            if (kv.Key == dmg.dmgDeltId)
            {
                pv = kv.Value.playerController.photonView;
                break;
            }
        }
        if (pv == null)
        {
            Debug.LogError("NULL PV");
        }

        playerController.photonView.RPC(nameof(OnDamageRecieved), pv.Owner, JsonUtility.ToJson(dmg));
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

        //Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(dmgInfo.dmgDeltId);
        Debug.Log("SHOULD ONLY BE PLAYER: I am the player: " + playerController.photonView.Owner.ActorNumber + "\nBut I should be: " + PhotonNetwork.LocalPlayer.ActorNumber);
        if(playerController.photonView.IsMine)
        {
            Debug.Log("The PV is mine (" + dmgInfo.dmgDeltId + ")and I was shot by " + dmgInfo.dmgDealerId);
        }
/*        else
        {
            return;
        }*/

        //    dmgInfo.dmgDeltId == playerController.photonView.Owner.ActorNumber)

        if (playerController.photonView.IsMine)
        {
            Debug.Log("My actor number: " + (playerController.photonView.IsMine ? PhotonNetwork.LocalPlayer.ActorNumber : playerController.photonView.Owner.ActorNumber)    );
            Debug.Log("The actor who was shot: " + dmgInfo.dmgDeltId);
            dmgInfo.dmgValue = 10f;
            playerHealth.TakeDamage(dmgInfo);
                
        }

   /*     if (*//*playerController.photonView.IsMine &&*//* 
            dmgInfo.dmgDeltId == playerController.photonView.Owner.ActorNumber)
        {
            Debug.Log("FROM STRUCT: I " + dmgInfo.dmgDeltId + " have been shot by: " + dmgInfo.dmgDealerId);
            Debug.Log("FROM OBJ: I am: " + playerController.photonView.Owner.ActorNumber);
            // TODO: Call HP damage
            // TODO: Calculate damage based on weapon type and bodypart hit
            
            playerHealth.TakeDamage(10.0f);
        }*/
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
    public void OnPlayerKilled(int killerTeamNumber, int killedTeamNumber)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            //TODO: calculate team scores.
            Game_RuntimeData.gameScore.killsPerTeam[killedTeamNumber] += 1;
        }
    }

    /// <summary>
    /// PunRPC callback. The master client has announced that the game has ended, and is telling you the score.
    /// </summary>
    [PunRPC]
    public void OnGameEnded(string gameScoreStructJSON)
    {
        s_GameScore gameScoreStruct = (s_GameScore)JsonUtility.FromJson(gameScoreStructJSON, typeof(s_GameScore));
        
        //TODO: store the data into DB?

        // MunishesScoreStuff.HereIsTheScore(gameScoreStruct);


        GameMode_Manager.gameIsRunning = false;
    }
}
