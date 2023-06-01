using Proyecto26;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        s_GameScore currentScore = Game_RuntimeData.gameScore;
        int player2Kills = currentScore.killsPerPlayer[1];
        putPlayerDetailOnFirebase.team_1player_1Detail("Munish", player2Kills, 20);
    }
}
