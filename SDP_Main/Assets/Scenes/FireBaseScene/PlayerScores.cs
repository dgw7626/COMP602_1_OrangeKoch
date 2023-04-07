//using System.Reflection.Metadata;
using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;

public class PlayerScores : MonoBehaviour
{
    public Text scoreText;
    public InputField nameText;

    private System.Random random = new System.Random();

    User user = new User();

    public static int playerScore;
    public static string playerName;

    // Start is called before the first frame update
    private void Start()
    {
        if(scoreText != null) {
         playerScore = random.Next(0, 101);
        scoreText.text = "Score: " + playerScore;
        }
   
    }

    public void OnSubmit()
    {
        playerName = nameText.text;
        PostToDatabase();
    }

    public void OnGetScore()
    {
         RetrieveFromDatabase();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + user.userScore;
    }

   private void PostToDatabase()
   {
    User user = new User();
    RestClient.Put("https://project-10bbb-default-rtdb.firebaseio.com/"+ playerName +".json", user);
   }

   private void RetrieveFromDatabase()
   {
        RestClient.Get<User>("https://project-10bbb-default-rtdb.firebaseio.com/"+ nameText.text +".json").Then(response =>
         {
             user = response;
             UpdateScore();
        });
   }
}
