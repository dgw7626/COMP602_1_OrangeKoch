using Photon.Realtime;
using System.Collections;


/// <summary>
/// Implemented by different game modes to handle different game logic.
/// </summary>
public interface IgameMode
{
    /// <summary>
    /// Use this to initialize values. Recommend calling StartGame after a delay to allow network instantiation.
    /// </summary>
    public void InitGame();
    /// <summary>
    /// Use to begin a match.
    /// </summary>
    public void StartGame();
    /// <summary>
    /// Use this to synchronize a clock across players.
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnOneSecondCountdown();
    /// <summary>
    /// Handle a player joining partway through a match.
    /// </summary>
    /// <param name="newPlayer"></param>
    /// <returns></returns>
    public IEnumerator OnPlayerEnterMatch(Photon.Realtime.Player newPlayer);
    /// <summary>
    /// Per frame game logic.
    /// </summary>
    public void OnPerFrameUpdate();
    /// <summary>
    /// Handle a player being killed. Respawn/kick/spectate etc...
    /// </summary>
    /// <param name="deathInfoStruct"></param>
    public void OnPlayerKilled(s_DeathInfo deathInfoStruct);
    /// <summary>
    /// A player has dropped.
    /// </summary>
    /// <param name="playerLeftMatch"></param>
    public void OnPlayerLeftMatch(Player playerLeftMatch);
    /// <summary>
    /// Track deaths and kills across sessions.
    /// </summary>
    /// <param name="deathInfoStruct"></param>
    public void OnScoreEvent(s_DeathInfo deathInfoStruct);
    /// <summary>
    /// Game over. Cleanup instances and runtime data.
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnStopGame();
}
