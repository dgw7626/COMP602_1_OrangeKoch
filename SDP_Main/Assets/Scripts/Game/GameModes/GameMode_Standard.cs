using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// The standard game mode. This is used by default, unless another mode is selected.
/// Players are divided into two teams, a timer is set, and players have until the timer runs out to 
/// get as many kills as possible. The team with the highest number of kills wins.
/// 
/// This is a native c# class, and is held statically by game_RuntimeData. Implements the IgameMode interface.
/// </summary>
// public class GameMode_Standard : IgameMode
public class GameMode_Standard : MonoBehaviour, IgameMode
{
    public const int MAX_GAME_TIME_SECONDS = 120;
    private const int NUM_TEAMS = 2;
    private const int INITIAL_SCORE = 0;
    private int numPlayers;
    public s_GameScore teamScores;
    
    /// <summary>
    /// Called by the gameModeManager instace from within the scene.
    /// Sets up the game and initializes values. 
    /// </summary>
    // Current Change
    // public void InitGame()
    // {
    //     numPlayers = 0;

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
        }

        //Divy players up into teams
        for (int i = 0; i < Game_RuntimeData.instantiatedPlayers.Count; i++)
        {
            numPlayers++;
            int team = 0;
            if (Game_RuntimeData.instantiatedPlayers[i].photonView.Owner.ActorNumber % 2 == 0)
                team = 1;

            Game_RuntimeData.teams[team].Add(Game_RuntimeData.instantiatedPlayers[i]);
            Game_RuntimeData.instantiatedPlayers[i].transform.GetChild(0).GetComponent<Player_3dModelManager>().SetTeamColour(team);
            Game_RuntimeData.instantiatedPlayers[i].teamNumber = team;
        }

        // Setup Score struct
        teamScores = new s_GameScore();
        teamScores.numPlayers = numPlayers;
        teamScores.killsPerTeam = new int[NUM_TEAMS];
        teamScores.deathsPerTeam = new int[NUM_TEAMS];
        teamScores.killsPerPlayer = new int[numPlayers];
        teamScores.deathsPerPlayer = new int[numPlayers];
        teamScores.teamNumbersByPlayer = new int[numPlayers];
        for (int i = 0; i < Game_RuntimeData.instantiatedPlayers.Count; i++)
        {
            teamScores.teamNumbersByPlayer[i] = Game_RuntimeData.instantiatedPlayers[i].teamNumber;
        }

            teamScores.numTeams = NUM_TEAMS;
        Game_RuntimeData.gameScore = teamScores;

        // Initialize Countdown
        if (PhotonNetwork.IsMasterClient)
        {
            GameMode_Manager.gameTime = MAX_GAME_TIME_SECONDS;
        }

        StartGame();
    }

    /// <summary>
    /// Unlocks player movement and starts the game. The timer is started by gameModeManager directley after this.
    /// </summary>
    public void StartGame()
    {
        Game_RuntimeData.matchIsRunning = true;

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

    /// <summary>
    /// Locks player controlls, synchronizes the scores and then tells the gameModeManager to quit the scene.
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnStopGame()
    {
        if (!Game_RuntimeData.matchIsRunning)
            yield break;

        Debug.Log("Game Stoped!");

        Game_RuntimeData.matchIsRunning = false;
       

        //Lock Controlls
        foreach (Player_MultiplayerEntity p in Game_RuntimeData.instantiatedPlayers)
        {
            p.playerController.IsInputLocked = true;
        }
        // Current changes
        // if (Game_RuntimeData.thisMachinesPlayersPhotonView.Owner.IsMasterClient)
        // {
        //     Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(
        //         nameof(Player_MultiplayerEntity.OnGameEnded),
        //         RpcTarget.All,
        //         JsonUtility.ToJson(teamScores)
        //     );
        // }

        //TODO: Cleanup

        s_GameScore score = Game_RuntimeData.gameScore;

        yield return new WaitForSeconds(1);

        Game_RuntimeData.gameMode_Manager.QuitMultiplayer();
    }

    /// <summary>
    /// Synchronous timer. Only the MasterClient decrements the time each second. Others will recieve the new time value from the network.
    /// After the timer has expired, starts the end of match cleanup.
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnOneSecondCountdown()
    {
        Debug.Log("Begin! ");

        // Current Change
        // while(GameMode_Manager.timerIsRunning)

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

        //Timer has run out, begin the game over proccess.
        Game_RuntimeData.gameMode_Manager.StartCoroutine(Game_RuntimeData.gameMode_Manager.gameMode.OnStopGame());
    }

    /// <summary>
    /// Use for any per-frame game logic.
    /// </summary>
    public void OnPerFrameUpdate() { }

    public void OnScoreEvent(int score, int teamNumber)
    {
        if (teamNumber > NUM_TEAMS || teamNumber < 0)
            Debug.LogError(
                "ERROR: Team " + teamNumber + " does not exist! Cannot assign points to team"
            );

    /// <summary>
    /// Only the Master Client will execute this method. Counts score locally, to be shared with others later.
    /// </summary>
    /// <param name="deathInfoStruct"></param>
    public void OnScoreEvent(s_DeathInfo deathInfoStruct)
    {
        teamScores.killsPerPlayer[deathInfoStruct.killerId-1]++;
        teamScores.deathsPerPlayer[deathInfoStruct.diedId-1]++;
        teamScores.killsPerTeam[deathInfoStruct.killerTeam]++;
        teamScores.deathsPerTeam[deathInfoStruct.diedTeam]++;

        Game_RuntimeData.gameScore = teamScores;
        Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(nameof(Player_MultiplayerEntity.UpdateScore), RpcTarget.All, JsonUtility.ToJson(teamScores));
    }

    /// <summary>
    /// Executed by everyone. This tells a machine that a particular player has been killed, so you should handle respawning if applicable.
    /// </summary>
    /// <param name="deathInfoStruct"></param>
        
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

    /// <summary>
    /// Use this to handle a player dropping.
    /// </summary>
    /// <param name="playerLeftMatch"></param>
    public void OnPlayerLeftMatch(Player playerLeftMatch)
    {
        // TODO: Check
        Game_RuntimeData.activePlayers.Remove(playerLeftMatch.ActorNumber);
    }

    /// <summary>
    /// If a player enters partway though, track them in the runtime data.
    /// </summary>
    /// <param name="newPlayer"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Respawns the player and performs health and ammunition updates.
    /// </summary>
    /// <param name="value">Key-value pair representing the player's unique identifier and corresponding multiplayer entity.</param>
    public void OnPlayerRespawn(KeyValuePair<int, Player_MultiplayerEntity> value)
    {

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
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == value.Key)
            {
                targetPlayer = p;
                break;
            }
        }
        //Call the "OnRespawn" method on the target player's multiplayer entity via RPC
        if (targetPlayer != null)
            if (targetPlayer != null)
                Game_RuntimeData.thisMachinesPlayersPhotonView.RPC(nameof(Player_MultiplayerEntity.OnRespawn), targetPlayer);
    }
}
