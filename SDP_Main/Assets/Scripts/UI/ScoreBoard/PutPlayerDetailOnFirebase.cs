using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Proyecto26;

public class PutPlayerDetailOnFirebase : MonoBehaviour 
{
    ScoreManager scoreMan1 = new ScoreManager("hg", 5, 0);
    ScoreManager scoreMan2 = new ScoreManager("John", 10, 10);

 // The start methode put the player detail on Firebase with help of RestClient.   
    void Start()
    {
     RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player1.json", scoreMan1);
     RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/player2.json", scoreMan2);    
    }

     void Update()
     {
          //ScoreManager("Sky", 5, 5);
          //ScoreManager("Harry", 10, 10);
          
         //RestClient.Delete("https://project-10bbb-default-rtdb.firebaseio.com/.json");
     }
    
}
