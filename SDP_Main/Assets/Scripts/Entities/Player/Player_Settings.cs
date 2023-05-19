using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Settings : MonoBehaviour
{
    public bool invertMouseYAxis;
    public int mouseMovementSpeed;
    public float globalVolume;
    public void InvertMouseYAxis()
    {
        // invertMouseYAxis = !invertMouseYAxis;
    }
    public void setMouseSpeed(int speed)
    {
        //mouseMovementSpeed = speed;
    }
    public void setGlobalVolume(float volume)
    {
        //mouseMovementSpeed = speed;
    }
    
}