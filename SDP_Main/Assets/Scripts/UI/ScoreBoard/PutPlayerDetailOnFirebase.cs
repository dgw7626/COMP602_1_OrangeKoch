using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;


public class PutPlayerDetailOnFirebase : MonoBehaviour
{
    

 // The start methode put the player detail on Firebase with help of RestClient.   
    void Start()
    {
       
    }

    // Team 1 Player 1 details goes on Firebase.
    public void team_1player_1Detail(string playerName, int kills, int deaths)
    {
        ScoreManager scoreMan1 = new ScoreManager(playerName, kills, deaths);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player1.json", scoreMan1);
    }

    // Team 1 Player 2 details goes on Firebase.
    public void team_1player_2Detail(string playerName, int kills, int deaths)
    {
        ScoreManager scoreMan2 = new ScoreManager(playerName, kills, deaths);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player2.json", scoreMan2);
    }

    // Team 1 Player 3 details goes on Firebase.
    public void team_1player_3Detail(string playerName, int kills, int deaths)
    {
        ScoreManager scoreMan3 = new ScoreManager(playerName, kills, deaths);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player3.json", scoreMan3);
    }

    // Team 2 Player 1 details goes on Firebase.
    public void team_2player_1Detail(string playerName, int kills, int deaths)
    {
        ScoreManager scoreMan4 = new ScoreManager(playerName, kills, deaths);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player4.json", scoreMan4);
    }

    // Team 2 Player 2 details goes on Firebase.
    public void team_2player_2Detail(string playerName, int kills, int deaths)
    {
        ScoreManager scoreMan5 = new ScoreManager(playerName, kills, deaths);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player5.json", scoreMan5);
    }

    // Team 2 Player 3 details goes on Firebase.
    public void team_2player_3Detail(string playerName, int kills, int deaths)
    {
        ScoreManager scoreMan6 = new ScoreManager(playerName, kills, deaths);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player6.json", scoreMan6);
    }


    // Winning team name put on Firebase.
    public void winningTeamName(int winningTeamName)
    {
        ScoreManager winningTeam = new ScoreManager(winningTeamName);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player7.json", winningTeam);

    }

    // Team 1 total kills
    public void team_1TotalKills(int team_1TotalKills)
    {
        ScoreManager totalKills = new ScoreManager(team_1TotalKills);
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player8.json", totalKills);
    }

   
    
}
