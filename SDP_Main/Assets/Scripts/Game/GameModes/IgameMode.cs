using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IgameMode
{
    public void InitGame();
    public void StartGame();
    public IEnumerator OnOneSecondCountdown();
    public IEnumerator OnPlayerEnterMatch(Photon.Realtime.Player newPlayer);
    public void OnPerFrameUpdate();
    public void OnPlayerKilled(s_DeathInfo deathInfoStruct);
    public void OnPlayerLeftMatch(Player playerLeftMatch);
    public void OnScoreEvent(s_DeathInfo deathInfoStruct);
    public IEnumerator OnStopGame();
    public void LeaveScene(string sceneName);
}
