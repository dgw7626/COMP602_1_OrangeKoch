using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Proyecto26;

public class PutPlayerDetailOnFirebase : MonoBehaviour 
{
    ScoreManager scoreMan1 = new ScoreManager("Jay", 5, 5, 5);
    ScoreManager scoreMan2 = new ScoreManager("Jayho", 10, 10, 10);
    
    void Start()
    {
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player1.json", scoreMan1);

        
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player2.json", scoreMan2);
    }

    
}
