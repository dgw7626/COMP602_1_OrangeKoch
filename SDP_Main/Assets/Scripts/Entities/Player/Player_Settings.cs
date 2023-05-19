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
    public void setMouseSpeed(float speedMultiplier)
    {
        //Check bounds
        if(speedMultiplier >= 0.5f && speedMultiplier <= 2.0f)
            lookSensitivity = speedMultiplier;
    }
    public void setGlobalVolume(float volume)
    {
        globalVolume = volume;
    }
    
}