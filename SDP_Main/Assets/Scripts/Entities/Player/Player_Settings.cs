using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Settings : MonoBehaviour
{
    public bool invertMouseYAxis = false;
    public float lookSensitivity = 1;
    public float globalVolume = 1;
    public void InvertMouseYAxis()
    {
         invertMouseYAxis = !invertMouseYAxis;
    }
    public void setInvertMouseYAxis(bool invert)
    {
        //Check bounds
        if (invert || !invert)
        {
            invertMouseYAxis = invert;
            Debug.Log("Invert Mouse Y Axis is set to: " + invertMouseYAxis);
        }
    }
    public void setMouseSpeed(float speedMultiplier)
    {
        //Check bounds
        if(speedMultiplier >= 0.25f && speedMultiplier <= 2.0f)
        {
            lookSensitivity = speedMultiplier;
            Debug.Log("Look Sensitivity Multiplier Set to: "+lookSensitivity);
        }
    }
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