using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_RuntimeData
{
    public static List<Player_MultiplayerEntity> activePlayers = new List<Player_MultiplayerEntity>();
    public static List<List<Player_MultiplayerEntity>> teams = new List<List<Player_MultiplayerEntity>>();
    public static bool isMultiplayer = true;
    public static IgameMode gameMode = null;

}
