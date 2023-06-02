using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreManagerUI : MonoBehaviour
{
    PutPlayerDetailOnFirebase putPlayerDetailOnFirebase = new PutPlayerDetailOnFirebase();
    ScoreBoard scoreBoard = new ScoreBoard();

    // Start is called before the first frame update
    void Start()
    {
        RestClient.Delete("https://project-10bbb-default-rtdb.firebaseio.com/.json");
        StartCoroutine(UpdateDbScores());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator UpdateDbScores()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            
            s_GameScore currentScore = Game_RuntimeData.gameScore;
            int player2Kills0 = currentScore.killsPerPlayer[0];
            int player1deaths0 = currentScore.deathsPerPlayer[0];
            int team1player10 = currentScore.teamNumbersByPlayer[0];
            string player0 = Convert.ToString(team1player10);
            putPlayerDetailOnFirebase.team_1player_1Detail(player0, player2Kills0, player1deaths0);

            if (currentScore.numPlayers < 2)
                continue;

            int player2Kills1 = currentScore.killsPerPlayer[1];
            int player1deaths1 = currentScore.deathsPerPlayer[1];
            int team1player11 = currentScore.teamNumbersByPlayer[1];
            string player1 = Convert.ToString(team1player11);
            putPlayerDetailOnFirebase.team_1player_2Detail(player1, player2Kills1, player1deaths1);

            if (currentScore.numPlayers < 3)
                continue;

            int player2Kills2 = currentScore.killsPerPlayer[2];
            int player1deaths2 = currentScore.deathsPerPlayer[2];
            int team1player12 = currentScore.teamNumbersByPlayer[2];
            string player2 = Convert.ToString(team1player12);
            putPlayerDetailOnFirebase.team_1player_2Detail(player2, player2Kills2, player1deaths2);

            if (currentScore.numPlayers < 4)
                continue;

            int player2Kills3 = currentScore.killsPerPlayer[3];
            int player1deaths3 = currentScore.deathsPerPlayer[3];
            int team1player13 = currentScore.teamNumbersByPlayer[3];
            string player3 = Convert.ToString(team1player13);
            putPlayerDetailOnFirebase.team_1player_2Detail(player3, player2Kills3, player1deaths3);

            if (currentScore.numPlayers < 5)
                continue;

            int player2Kills4 = currentScore.killsPerPlayer[4];
            int player1deaths4 = currentScore.deathsPerPlayer[4];
            int team1player14 = currentScore.teamNumbersByPlayer[4];
            string player4 = Convert.ToString(team1player14);
            putPlayerDetailOnFirebase.team_1player_2Detail(player4, player2Kills4, player1deaths4);

            if (currentScore.numPlayers < 6)
                continue;

            int player2Kills5 = currentScore.killsPerPlayer[5];
            int player1deaths5 = currentScore.deathsPerPlayer[5];
            int team1player15 = currentScore.teamNumbersByPlayer[5];
            string player5 = Convert.ToString(team1player15);
            putPlayerDetailOnFirebase.team_1player_2Detail(player5, player2Kills5, player1deaths5);

            if (currentScore.numPlayers < 7)
                continue;

        }
    }
}
