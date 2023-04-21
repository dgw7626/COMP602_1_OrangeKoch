using System.Collections;
using System.Collections.Generic;
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

    public static List<string> GetGameMapScenes()
    {
        List<string> MapList = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                string s = scene.path; //Temporarily store the Scene Path as String
                if (!s.Contains(Data_Scenes.GameMap_Prefix)) //Check the scene name to ensure include GameMap Prefix
                    continue;

                s = s.Split('/')[2]; //Remove the file path before the script name
                s = s.Split('.')[0]; //Remove .unity from the string
                MapList.Add(s); //Add the GameMap String to a MapList
            }
        }
        if(MapList.Count < 1)
            Debug.LogError("No Game Maps Found.");
        return MapList;
    }

}
