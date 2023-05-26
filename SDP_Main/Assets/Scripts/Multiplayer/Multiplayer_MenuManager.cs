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
/// This class is designed Manage a list of the Menus within the Multiplayer UI
/// </summary>
public class Multiplayer_MenuManager : MonoBehaviour
{
    public static Multiplayer_MenuManager Instance;

    [SerializeField] Multiplayer_MenuItem[] menus;

    /// <summary>
    /// This method instantiates an instance of the Menu Manager
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This method is designed to open a Menu by searching the Menu List and matching it with a String parameter.
    /// </summary>
    public void OpenMenu(string menuName)
    {
        for(int i=0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    /// <summary>
    /// This method is designed to open a Menu by searching the Menu List and matching it with a Menu Object parameter.
    /// </summary>
    public void OpenMenu(Multiplayer_MenuItem menu)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    /// <summary>
    /// This method is designed to close a Menu by searching the Menu List and matching it with a Menu Object parameter.
    /// </summary>
    public void CloseMenu(Multiplayer_MenuItem menu)
    {
        menu.Close();
    }

}
