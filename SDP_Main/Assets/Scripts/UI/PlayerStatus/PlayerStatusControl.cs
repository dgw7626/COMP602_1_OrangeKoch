using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class PlayerStatusControl : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;
    public GameObject player6;
    public Text teamScore1;
    public Text teamScore2;
    int team_1_kills = 0;
    int team_2_kills = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            player_1_kill();
            AddScoreTeam_2();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            player_2_kill();
            AddScoreTeam_2();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            player_3_kill();
            AddScoreTeam_2();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            player_4_kill();
            AddScoreTeam_1();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            player_5_kill();
            AddScoreTeam_1();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            player_6_kill();
            AddScoreTeam_1();
        }
    }

    public void AddScoreTeam_1()
    {
        team_1_kills++;
        teamScore1.text = team_1_kills.ToString();

    }

    public void AddScoreTeam_2()
    {
        team_2_kills++;
        teamScore2.text = team_2_kills.ToString();
    }
    public void hideAllPlayers()
    {
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        player6.SetActive(false);
    }

    public void player_1_kill()
    {
        player1.SetActive(false);
    }

    public void player_2_kill()
    {
        player2.SetActive(false);
    }

    public void player_3_kill()
    {
        player3.SetActive(false);
    }

    public void player_4_kill()
    {
        player4.SetActive(false);
    }

    public void player_5_kill()
    {
        player5.SetActive(false);
    }

    public void player_6_kill()
    {
        player6.SetActive(false);
    }
}
