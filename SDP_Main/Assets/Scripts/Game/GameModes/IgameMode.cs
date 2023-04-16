using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IgameMode
{
    public void InitGame();
    public void StartGame();
    public IEnumerator OnOneSecondCountdown();
    public void OnPerFrameUpdate();
    public void OnPlayerKilled(Player_MultiplayerEntity playerKilled);
    public void OnPlayerLeftMatch(Player playerLeftMatch);
    public IEnumerator OnPlayerEnterMatch(Photon.Realtime.Player newPlayer);
    public void OnScoreEvent(int score, int teamNumber);
    public void OnStopGame();
    public void LeaveScene(string sceneName);
}
