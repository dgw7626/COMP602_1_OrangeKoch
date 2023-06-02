/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Corey Knigth	            *
 * Student ID: 		21130891		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An instance of this will exist inside multiplayer scenes. Used to create a c# native gameMode, and start coroutines.
/// </summary>
public class GameMode_Manager : MonoBehaviourPunCallbacks
{
    private const float GAME_START_DELAY_SECONDS = 0.8f;
    public IgameMode gameMode;
    public static int gameTime = 999;
    public static bool timerIsRunning = false;

    /// <summary>
    ///Fetch the gameMode from Game_RuntimeData and Invoke InitGame on that gameMode.
    ///
    /// If GameMode is null, it will be set to the default game mode.
    ///
    /// The delay before Init() gives the Player_MultiplayerEntity's time to
    /// instantiate and register themselves with Game_RunTimeData.
    /// </summary>
    void Awake()
    {
        Game_RuntimeData.gameMode_Manager = this;

        if(Game_RuntimeData.gameMode == null)
        {
            Game_RuntimeData.gameMode = new GameMode_Standard();
        }
        gameMode = Game_RuntimeData.gameMode;

        Invoke("Init", GAME_START_DELAY_SECONDS);
    }


    /// <summary>
    /// Calls the GameMode's Init after a delay(GAME_START_DELAY_SECONDS),
    /// and starts the game Timer.
    /// </summary>
    void Init()
    {
        Game_RuntimeData.activePlayers = new Dictionary<int, Player_MultiplayerEntity>();

        foreach (Player_MultiplayerEntity e in Game_RuntimeData.instantiatedPlayers)
        {
            Game_RuntimeData.RegisterNewMultiplayerPlayer(e.GetComponent<PhotonView>().Owner.ActorNumber, e);
            e.playerHealth.Begin(e);
        }
        gameMode.InitGame();

        timerIsRunning = true;

        //Begin the synchronous timer
        StartCoroutine(gameMode.OnOneSecondCountdown());

    }

    /// <summary>
    ///  Calls the GameModes perFrameUpdate method.
    /// </summary>
    void Update()
    {
        gameMode.OnPerFrameUpdate();
    }

    /// <summary>
    /// Allow a new player to register themselves with the runtimeData, then tell GameMode to handle their entry.
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        StartCoroutine(gameMode.OnPlayerEnterMatch(newPlayer));

    }
    public override void OnJoinedRoom()
    {
    }

    /// <summary>
    /// Tell the network, and the gameMode, that a player has dropped.
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        gameMode.OnPlayerLeftMatch(otherPlayer);

        Debug.Log("Player" +  otherPlayer.ActorNumber + " left the room!");
        Debug.Log("The master client is: " + PhotonNetwork.MasterClient.ActorNumber);
    }

    /// <summary>
    /// Decrement the timer value. Only the master client will do this. Called by RPC
    /// </summary>
    public static void SetSynchronousTimerValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameTime--;
        }
    }


    /// <summary>
    /// Method to quit multiplayer and return to Main Lobby.
    /// </summary>
    public void QuitMultiplayer()
    {
        Game_RuntimeData.CleanUp_Multiplayer_Data();
        // Get a reference to the Photon Voice Manager object
        var voiceManager = GameObject.Find("VoiceManager");
        // If the object exists, stop and destroy the voice service
        if (voiceManager != null)
        {
            Destroy(voiceManager);
        }
        StartCoroutine(QuitAfterDelay());
    }

    /// <summary>
    /// Method to quit single player and return to Main Lobby.
    /// </summary>
    public void QuitSinglePlayer()
    {
        Game_RuntimeData.CleanUp_Multiplayer_Data();
        Game_GameState.NextScene("Lobby");
    }

    /// <summary>
    /// Method to delay quitting and wait to disconnect from Photon Server.
    /// </summary>
    IEnumerator QuitAfterDelay()
    {
        while (true)
        {
            if (!PhotonNetwork.IsConnected)
            {
                break;
            }
            yield return null;
        }
        Game_GameState.NextScene("MultiplayerScoreboard");
    }
}
