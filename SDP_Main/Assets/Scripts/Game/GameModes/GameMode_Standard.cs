using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using System;

public class GameMode_Standard : IgameMode
{
    public const int MAX_GAME_TIME_SECONDS = 120;
    private const int NUM_TEAMS = 2;
    private const int INITIAL_SCORE = 0;
    private int numPlayers;
    public List<int> teamScores {  get; private set; }
    public void InitGame()
    {
        // Initialize Countdown

        teamScores = new List<int>();

        // Make teams
        for (int i = 0; i < NUM_TEAMS; i++)
        {
            Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
            teamScores.Add(INITIAL_SCORE);
        }

        //Divy players up into teams
        for (int i = 0; i < Game_RuntimeData.instantiatedPlayers.Count; i++)
        {
            numPlayers++;
            int team = i % 2 == 0 ? 0 : 1;

            Game_RuntimeData.teams[team].Add(Game_RuntimeData.instantiatedPlayers[i]);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            GameMode_Manager.InitializeGameTimer(MAX_GAME_TIME_SECONDS);
        }

        StartGame();
    }

    public void StartGame()
    {
        Game_RuntimeData.DebugPrintMP_PlayerInfo();
        // Unlock Player Movement
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.instantiatedPlayers)
        {
            p.playerController.IsMultiplayer = true;
            p.playerController.IsInputLocked = false;
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
        //TODO: Cleanup
    }

    public IEnumerator OnOneSecondCountdown()
    {
        Debug.Log("Begin! ");
        while(true)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //Game_RuntimeData.thisMachinesPlayersPhotonView.RPC("SetSynchronousTimerValue", RpcTarget.All, new Object[0]);
                GameMode_Manager.SetSynchronousTimerValue();
            }
            else
            {
                //Game_RuntimeData.thisMachinesPlayersPhotonView.RPC("GetSynchronousTimerValue", PhotonNetwork.MasterClient, new Object[0]);
            }
            //Debug.Log("Time left: " + gameTime);
            yield return new WaitForSeconds(1);

        }

        OnStopGame();
    }

    public void OnPerFrameUpdate()
    {
    }
    public void OnScoreEvent(int score, int teamNumber)
    {
        if (teamNumber > NUM_TEAMS || teamNumber < 0)
            Debug.LogError("ERROR: Team " + teamNumber + " does not exist! Cannot assign points to team");

        teamScores[teamNumber] += score;
    }
    public void OnPlayerKilled(Player_MultiplayerEntity playerKilled)
    {
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
                Game_RuntimeData.RegisterNewMultiplayerPlayer(e.GetComponent<PhotonView>().Owner.ActorNumber, e);
            }
        }
    }


}
