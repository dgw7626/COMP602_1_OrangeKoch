using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Game_GameState
{
    public static AbstractState state;

    public static void RunCurrentState(System.Object param)
    {
        state = state.RunState(param);
    }

    /// <summary>
    /// Transition to a new scene
    /// </summary>
    public static void NextScene(string sceneName)
    {
        //Get number of scenes in build settings
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        Debug.Log("Loading next scene: " + sceneName);

        //Loop through all scenes
        for (int i = 0; i < sceneCount; i++)
        {
            //set temp scene name
            string tmpScene = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)); //Temporarily store the Scene Path as String

            //Check temp scene if matching desired scene to change to
            if (tmpScene == sceneName)
            {
                SceneManager.LoadScene(i);
                return;
            }
        }
        Debug.LogError("Failed to load scene: " + sceneName);
    }

    /// <summary>
    /// Return a list of GameMap Strings derived from Scenes
    /// </summary>
    public static List<string> GetGameMapScenes()
    {
        List<string> MapList = new List<string>();

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string s = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)); //Temporarily store the Scene Path as String
            if (!s.Contains(Data_Scenes.GameMap_Prefix)) //Check the scene name to ensure include GameMap Prefix
                continue;

            Debug.Log("Game Map: " + s + " add to Map List.");
            MapList.Add(s); //Add the GameMap String to a MapList
        }

        if (MapList.Count < 1)
            Debug.LogError("No Game Maps Found.");
        return MapList;
    }

}
