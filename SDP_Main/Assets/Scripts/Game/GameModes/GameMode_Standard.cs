using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

public class GameMode_Standard : IgameMode
{
    public const int MAX_GAME_TIME_SECONDS = 20;
    private const int NUM_TEAMS = 2;
    private const int INITIAL_SCORE = 0;
    private int numPlayers;
    public int _gameTime {  get; private set; }
    public List<int> teamScores {  get; private set; }
    public void InitGame()
    {
        // Initialize Countdown
        _gameTime = MAX_GAME_TIME_SECONDS + 1;

        // Make teams
        for(int i = 0; i < NUM_TEAMS; i++)
        {
            Game_RuntimeData.teams.Add(new List<Player_MultiplayerEntity>());
            teamScores.Add(INITIAL_SCORE);
        }

        //Divy players up into teams
        for(int i = 0; i < Game_RuntimeData.activePlayers.Count; i++)
        {
            numPlayers++;
            int team = i % 2 == 0 ? 0 : 1;

            Game_RuntimeData.teams[team].Add(Game_RuntimeData.activePlayers[i]);
        }

        StartGame();
    }

    public void StartGame()
    {
        // Unlock Player Movement
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.activePlayers)
        {
            p.playerController.IsMultiplayer = true;
            p.playerController.IsInputLocked = false;
        }
    }

    public void OnStopGame()
    {
        Debug.Log("Game Stoped!");

        //Lock Controlls
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.activePlayers)
        {
            p.playerController.IsInputLocked = true;
        }
        //TODO: Cleanup
    }

    public IEnumerator OnOneSecondCountdown()
    {
        Debug.Log("Begin! ");
        while(_gameTime > 0)
        {
            _gameTime--;
            Debug.Log("Time left: " + _gameTime);
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

    public void OnPlayerLeftMatch(Player_MultiplayerEntity playerLeftMatch)
    {
        throw new System.NotImplementedException();
    }
}
