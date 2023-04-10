using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game_GameState
{
    public static AbstractState state;

    public static void RunCurrentState(System.Object param)
    {
        state = state.RunState(param);
    }
}
