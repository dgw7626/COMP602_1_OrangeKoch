using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Game_GameState
{
    public static AbstractState state;

    public static void RunCurrentState(System.Object param)
    {
        state = state.RunState(param);
    }

    public static void NextScene(string sceneName)
    {
        Debug.Log("Loading next scene: " + sceneName);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                SceneManager.LoadScene(i);
                return;
            }
        }
        Debug.LogError("Failed to load scene: " + sceneName);
    }

}
