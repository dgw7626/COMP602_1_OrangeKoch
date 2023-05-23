using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using System;
using Newtonsoft.Json.Schema;

public class GameMode_Standard : IgameMode
{
    public const int MAX_GAME_TIME_SECONDS = 120;
    private const int NUM_TEAMS = 2;
    private const int INITIAL_SCORE = 0;
    private int numPlayers;
    public s_GameScore teamScores;
    public void InitGame()
    {


        // Make teams
        for (int i = 0; i < NUM_TEAMS; i++)
        {
            Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
        }

        //Divy players up into teams
        for (int i = 0; i < Game_RuntimeData.instantiatedPlayers.Count; i++)
        {
            numPlayers++;
            int team = 0;
            if (Game_RuntimeData.instantiatedPlayers[i].photonView.Owner.ActorNumber % 2 != 0)
                team = 1;

            Game_RuntimeData.teams[team].Add(Game_RuntimeData.instantiatedPlayers[i]);
            Game_RuntimeData.instantiatedPlayers[i].transform.GetChild(0).GetComponent<Player_3dModelManager>().SetTeamColour(team);
            Game_RuntimeData.instantiatedPlayers[i].teamNumber = team;
        }

        // Setup Score struct
        teamScores = new s_GameScore();
        teamScores.killsPerTeam = new int[NUM_TEAMS];
        teamScores.deathsPerTeam = new int[NUM_TEAMS];
        teamScores.killsPerPlayer = new int[numPlayers];
        teamScores.deathsPerPlayer = new int[numPlayers];
        teamScores.numTeams = NUM_TEAMS;
        Game_RuntimeData.gameScore = teamScores;

        // Initialize Countdown
        if (PhotonNetwork.IsMasterClient)
        {
            GameMode_Manager.gameTime =  MAX_GAME_TIME_SECONDS;
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
      
            if(p.playerController.photonView.IsMine)
            {
                Game_RuntimeData.thisMachinesPlayersPhotonView = p.playerController.photonView;
                Debug.Log("At START GAME: ThisMachines PhotonView is mine, and my number is: " + PhotonNetwork.LocalPlayer.ActorNumber + 
                    " " + Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.ActorNumber);
            }
        }

    }

    public IEnumerator OnStopGame()
    {
        Debug.Log("Game Stoped!");

        //Lock Controlls
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.instantiatedPlayers)
        {
            p.playerController.IsInputLocked = true;
        }
        //Share Score
        if(Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.IsMasterClient)
        {
            Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(nameof(Player_MultiplayerEntity.UpdateScore), RpcTarget.Others, JsonUtility.ToJson(teamScores));
        }

        //TODO: Cleanup

        //TODO: Score Board
        s_GameScore score = Game_RuntimeData.gameScore;
        Debug.Log("player 1 killed: " + score.killsPerPlayer[0]);
        Debug.Log("player 1 dies: " + score.deathsPerPlayer[0]);
        Debug.Log("player 2 killed: " + score.killsPerPlayer[1]);
        Debug.Log("player 2 dies: " + score.deathsPerPlayer[1]);
        Debug.Log("team 1 killed: " + score.killsPerTeam[0]);
        Debug.Log("team 1 died: " + score.deathsPerTeam[0]);
        Debug.Log("team 2 killed: " + score.killsPerTeam[1]);
        Debug.Log("team 2 died: " + score.deathsPerTeam[1]);
        
        yield return new WaitForSeconds(3);

        Game_RuntimeData.gameMode_Manager.QuitMultiplayer();
    }

    public IEnumerator OnOneSecondCountdown()
    {
        Debug.Log("Begin! ");
        while(GameMode_Manager.gameIsRunning)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                GameMode_Manager.SetSynchronousTimerValue();
                Game_RuntimeData.thisMachinesPlayersPhotonView.RPC("GetSynchronousTimerValue", RpcTarget.Others, GameMode_Manager.gameTime);

                if(GameMode_Manager.gameTime < 1)
                    GameMode_Manager.gameIsRunning = false;
            }

            Debug.Log(GameMode_Manager.gameTime);
            yield return new WaitForSeconds(1);

        }

        Game_RuntimeData.gameMode_Manager.StartCoroutine(Game_RuntimeData.gameMode_Manager.gameMode.OnStopGame());
    }

    public void OnPerFrameUpdate()
    {
    }
    public void OnScoreEvent(s_DeathInfo deathInfoStruct)
    {
        teamScores.killsPerPlayer[deathInfoStruct.killerId-1]++;
        teamScores.deathsPerPlayer[deathInfoStruct.diedId-1]++;
        teamScores.killsPerTeam[deathInfoStruct.killerTeam]++;
        teamScores.deathsPerTeam[deathInfoStruct.diedTeam]++;
    }
    public void OnPlayerKilled(s_DeathInfo deathInfoStruct)
    {
        //TODO: Find player and respwan/destroy them here
        foreach(KeyValuePair<int, Player_MultiplayerEntity> value in Game_RuntimeData.activePlayers)
        {
            if(value.Key == deathInfoStruct.diedId) 
            {
                PhotonView pv = value.Value.playerController.photonView;
                value.Value.gameObject.transform.position = new Vector3(0, 0, 0);
                return;
            }
        }
    }

    public void LeaveScene(string sceneName)
    {

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
