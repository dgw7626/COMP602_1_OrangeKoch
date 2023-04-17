using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplayer_MenuManager : MonoBehaviour
{
    public static Multiplayer_MenuManager Instance;

    [SerializeField] Multiplayer_MenuItem[] menus;

    private void Awake()
    {
        Instance = this;
    }

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

    public void CloseMenu(Multiplayer_MenuItem menu)
    {
        menu.Close();
    }

}
