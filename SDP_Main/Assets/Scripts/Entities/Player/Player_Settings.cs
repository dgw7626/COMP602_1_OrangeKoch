/*

 ************************************************
 *                                              *
 * Primary Dev: 	Dion Hemmes		            *
 * Student ID: 		21154191		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *
 ************************************************

 */
using UnityEngine;


/// <summary>
/// This Class holds global settings for the player which can be accessed through Game_RuntimeData
/// </summary>
public class Player_Settings
{
    public bool invertMouseYAxis = false;
    public float lookSensitivity = 1;
    public float globalVolume = 1;

    /// <summary>
    /// Method to invert the Mouse Y Axis setting for the player.
    /// </summary>
    public void InvertMouseYAxis()
    {
         invertMouseYAxis = !invertMouseYAxis;
    }

    /// <summary>
    /// Method to set the Mouse Y Axis inversion setting for the player.
    /// </summary>
    public void setInvertMouseYAxis(bool invert)
    {
        //Check bounds
        if (invert || !invert)
        {
            invertMouseYAxis = invert;
            Debug.Log("Invert Mouse Y Axis is set to: " + invertMouseYAxis);
        }
    }

    /// <summary>
    /// Method to set the MouseSpeed/Look Sensitivity setting for the player.
    /// </summary>
    public void setMouseSpeed(float speedMultiplier)
    {
        //Check bounds
        if(speedMultiplier >= 0.25f && speedMultiplier <= 2.0f)
        {
            lookSensitivity = speedMultiplier;
            Debug.Log("Look Sensitivity Multiplier Set to: "+lookSensitivity);
        }
    }

    /// <summary>
    /// Method to set the master volume setting for the player.
    /// </summary>
    public void setGlobalVolume(float volume)
    {
        //Check bounds
        if (volume >= 0.0001f && volume <= 1.0f)
        {
            globalVolume = volume;
            Debug.Log("Global Volume Set to: " + globalVolume);
        }
    }
}
