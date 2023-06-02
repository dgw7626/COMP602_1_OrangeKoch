/*

 ************************************************
 *                                              *
 * Primary Dev: 	Corey Knigth	            *
 * Student ID: 		21130891		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *
 ************************************************

*/
using UnityEngine;

/// <summary>
/// This class manages the 3D model for the player.
/// </summary>
public class Player_3dModelManager : MonoBehaviour
{
    public Material purple;
    public Material orange;
    private Renderer myRenderer;
    
    /// <summary>
    /// Called once after instantiation
    /// </summary>
    void Start()
    {
        myRenderer = transform.GetChild(0).GetComponent<Renderer>();
        if(myRenderer ==  null )
        {
            Debug.LogError("Null Renderer found on player " + gameObject.name);
        }
    }

    /// <summary>
    /// Allocate either Purple or Orange colour to the model, based on team.
    /// </summary>
    /// <param name="teamNumber"></param>
    public void SetTeamColour(int teamNumber)
    {
        if (teamNumber == 0)
            myRenderer.material = orange;
        else
            myRenderer.material = purple;
    }
}
