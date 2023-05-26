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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to open an close windows as required within the Multiplayer UI
/// </summary>
public class Multiplayer_MenuItem : MonoBehaviour
{

    public string menuName;
    public bool open;


    /// <summary>
    /// Method to Open Windows by changing their Active Status
    /// </summary>
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Method to Close Windows by changing their Active Status
    /// </summary>
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
