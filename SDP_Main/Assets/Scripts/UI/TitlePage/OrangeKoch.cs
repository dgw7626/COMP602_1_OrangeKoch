/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Munish Kumar                *
 * Student ID: 		19083476		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SceneManagement;

public class OrangeKoch : MonoBehaviour
{
    public string Main_Menu = "Assets/Scenes/MainMenuUI.unity";
    public bool loadNextSceneInvoked = false;

    /// <summary>
    /// The start method load the main page after 3secends.
    /// </summary>
    public void Start()
    {
        Invoke("LoadNextScene", 3f);
    }

    /// <summary>
    /// This method load the next scene.
    /// </summary>
    public  void LoadNextScene()
    {
        loadNextSceneInvoked = true;
        SceneManager.LoadScene(Main_Menu);
    }

    /// <summary>
    /// This method check if there is next scene
    /// </summary>
    /// <returns> If next secen, it return true </returns>
    public bool IsLoadNextSceneInvoked()
    {
        return loadNextSceneInvoked;
    }
}
