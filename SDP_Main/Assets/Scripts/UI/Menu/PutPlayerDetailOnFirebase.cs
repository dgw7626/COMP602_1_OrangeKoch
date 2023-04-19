using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Proyecto26;

public class PutPlayerDetailOnFirebase : MonoBehaviour 
{
     public static string playerName = "Jayho";
     public static int playerKills = 3;
     public static int playerDeaths = 3;
     public static int playerWon = 3;

    void Start()
    {
        ScoreManager scoreMan = new ScoreManager();
        RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/Jayho.json", scoreMan);
    }

    
}
