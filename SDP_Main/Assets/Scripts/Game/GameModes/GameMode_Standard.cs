using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using System;
using Newtonsoft.Json.Schema;

public class GameMode_Standard : MonoBehaviour, IgameMode
{
    public const int MAX_GAME_TIME_SECONDS = 120;
    private const int NUM_TEAMS = 2;
    private const int INITIAL_SCORE = 0;
    private int numPlayers;
    public s_GameScore teamScores;

    // Kevin add: if make any problem, please delete that.
    private Player_Health playerHealth;
    private Weapon_ProjectileManager weapon_ProjectileManager;
    private Coroutine Coroutine;
    public void InitGame()
    {
        teamScores = new s_GameScore();
        teamScores.killsPerTeam = new List<int>();

        // Make teams
        for (int i = 0; i < NUM_TEAMS; i++)
        {
            Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
            teamScores.killsPerTeam.Add(INITIAL_SCORE);
        }

        //Divy players up into teams
        for (int i = 0; i < Game_RuntimeData.instantiatedPlayers.Count; i++)
        {
            numPlayers++;
            int team = i % 2 == 0 ? 0 : 1;

            Game_RuntimeData.teams[team].Add(Game_RuntimeData.instantiatedPlayers[i]);
        }

        // Initialize Countdown
        if (PhotonNetwork.IsMasterClient)
        {
            GameMode_Manager.gameTime = MAX_GAME_TIME_SECONDS;
        }

        StartGame();
    }

    public void StartGame()
    {
        Game_RuntimeData.DebugPrintMP_PlayerInfo();
        // Unlock Player Movement
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.instantiatedPlayers)
        {
            p.playerController.IsInputLocked = false;

            if (p.playerController.photonView.IsMine)
            {
                Game_RuntimeData.thisMachinesPlayersPhotonView = p.playerController.photonView;
                Debug.Log(
                    "At START GAME: ThisMachines PhotonView is mine, and my number is: "
                        + PhotonNetwork.LocalPlayer.ActorNumber
                        + " "
                        + Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber
                );
            }
        }
    }

    public void OnStopGame()
    {
        Debug.Log("Game Stoped!");

        //Lock Controlls
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.instantiatedPlayers)
        {
            p.playerController.IsInputLocked = true;
        }
        if (Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.IsMasterClient)
        {
            Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(
                nameof(Player_MultiplayerEntity.OnGameEnded),
                RpcTarget.All,
                JsonUtility.ToJson(teamScores)
            );
        }

        //TODO: Cleanup

        //TODO: Score Board
        Game_RuntimeData.gameMode_Manager.QuitMultiplayer();
    }

    public IEnumerator OnOneSecondCountdown()
    {
        Debug.Log("Begin! ");
        while (GameMode_Manager.gameIsRunning)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameMode_Manager.SetSynchronousTimerValue();
                Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(
                    "GetSynchronousTimerValue",
                    RpcTarget.Others,
                    GameMode_Manager.gameTime
                );

                // If the timer expires, tell the other players what the score is.
                if (GameMode_Manager.gameTime < 1)
                {
                    Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(
                        nameof(Player_MultiplayerEntity.OnGameEnded),
                        RpcTarget.Others,
                        JsonUtility.ToJson(Game_RuntimeData.gameScore)
                    );

                    GameMode_Manager.gameIsRunning = false;
                }
            }

            //Debug.Log(GameMode_Manager.gameTime);
            yield return new WaitForSeconds(1);
        }

        OnStopGame();
    }

    public void OnPerFrameUpdate() { }

    public void OnScoreEvent(int score, int teamNumber)
    {
        if (teamNumber > NUM_TEAMS || teamNumber < 0)
            Debug.LogError(
                "ERROR: Team " + teamNumber + " does not exist! Cannot assign points to team"
            );

        teamScores.killsPerTeam[teamNumber] += score;
    }

    public void OnPlayerKilled(s_DeathInfo deathInfoStruct)
    {
        //TODO: Find player and respwan/destroy them here
        foreach (
            KeyValuePair<int, Player_MultiplayerEntity> value in Game_RuntimeData.activePlayers
        )
        {
            if (value.Key == deathInfoStruct.diedId)
            {
                OnPlayerRespawn(value);
                return;
            }
        }
    }

    public void LeaveScene(string sceneName)
    {
        Game_GameState.NextScene(sceneName);
    }

    public void OnPlayerLeftMatch(Player playerLeftMatch)
    {
        // TODO: Check
        Game_RuntimeData.activePlayers.Remove(playerLeftMatch.ActorNumber);
    }

    public IEnumerator OnPlayerEnterMatch(Player newPlayer)
    {
        yield return new WaitForSeconds(0.5f);

        int id = newPlayer.ActorNumber;

        foreach (Player_MultiplayerEntity e in Game_RuntimeData.instantiatedPlayers)
        {
            if (e.GetComponent<PhotonView>().Owner.ActorNumber == id)
            {
                Game_RuntimeData.RegisterNewMultiplayerPlayer(
                    e.GetComponent<PhotonView>().Owner.ActorNumber,
                    e
                );
            }
        }
    }

    public void OnPlayerRespawn(KeyValuePair<int, Player_MultiplayerEntity> value)
    {
        //TODO: detroy the gameobject
        // GameObject.Destroy(value.Value.gameObject);
        //TODO: create a new one
        //   Multiplayer_PlayerManager.CreateController();
        Player_Health playerHealth = value.Value.GetComponent<Player_Health>();
        Weapon_ProjectileManager weapon_ProjectileManager = value.Value.gameObject.GetComponentInChildren<Weapon_ProjectileManager>();
        //update health
        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.currentUIHealth = playerHealth.maxHealth;
        playerHealth.healthBar.SetHealth(playerHealth.currentUIHealth);
        //Update the ammunition
        
        weapon_ProjectileManager._weaponAmmo = weapon_ProjectileManager._weaponInfo.BulletCounts;
        weapon_ProjectileManager._weaponClip = weapon_ProjectileManager._weaponInfo.ClipCounts;

        weapon_ProjectileManager._ammunitionUI.SetAmmunition(
             weapon_ProjectileManager._weaponAmmo,
             weapon_ProjectileManager._weaponClip
         );
        // update the respawn point
        value.Value.gameObject.transform.position = new Vector3(0, 30, 0);
        //set the invincible time
        Player targetPlayer = null;
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if(p.ActorNumber == value.Key)
            {
                targetPlayer = p;
                break;
            }
        }

        if (targetPlayer != null)
            Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(nameof(Player_MultiplayerEntity.OnRespawn), targetPlayer);
    }
}
