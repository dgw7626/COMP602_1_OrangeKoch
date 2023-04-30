using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Manager : MonoBehaviourPunCallbacks
{
    private const float GAME_START_DELAY_SECONDS = 0.8f;
    public IgameMode gameMode;
    public static int gameTime;


    /**
     * Fetch the gameMode from Game_RuntimeData and Invoke InitGame on that gameMode.
     * 
     * If GameMode is null, it will be set to the default game mode.
     * 
     * The delay before Init() gives the Player_MultiplayerEntity's time to 
     * instantiate and register themselves with Game_RunTimeData.
     */
    void Awake()
    {
        if(Game_RuntimeData.gameMode == null)
        {
            Game_RuntimeData.gameMode = new GameMode_Standard();
        }
        gameMode = Game_RuntimeData.gameMode;

        Invoke("Init", GAME_START_DELAY_SECONDS);
    }

    /**
     * Calls the GameMode's Init after a delay(GAME_START_DELAY_SECONDS), 
     * and starts the game Timer.
     */
    void Init()
    {
        Game_RuntimeData.activePlayers = new Dictionary<int, Player_MultiplayerEntity>();

        foreach (Player_MultiplayerEntity e in Game_RuntimeData.instantiatedPlayers)
        {
            //if (e.GetComponent<PhotonView>().IsMine)
            {
                Game_RuntimeData.RegisterNewMultiplayerPlayer(e.GetComponent<PhotonView>().Owner.ActorNumber, e);
            }
        }
        gameMode.InitGame();


        StartCoroutine(gameMode.OnOneSecondCountdown());
    }

    /**
     * Calls the GameModes perFrameUpdate method.
     */
    void Update()
    {
        gameMode.OnPerFrameUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        StartCoroutine(gameMode.OnPlayerEnterMatch(newPlayer));
        
    }
    public override void OnJoinedRoom()
    {
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        gameMode.OnPlayerLeftMatch(otherPlayer);
    }
    public static void SetSynchronousTimerValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameTime--;
        }
    }
}
