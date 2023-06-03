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
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // Show the cursor
        Cursor.visible = true;

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;

        //Set multiplayer to false
        Game_RuntimeData.isMultiplayer = false;
    }
    public void Multiplayer()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        SceneManager.LoadScene("MultiplayerMenu");
    }
    public void Tutorial ()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("TutorialMap");
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

}
