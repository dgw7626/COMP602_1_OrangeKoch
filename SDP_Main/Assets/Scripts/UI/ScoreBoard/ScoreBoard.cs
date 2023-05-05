using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Proyecto26;
using System;
using System.Net.Mime;

public class ScoreBoard : MonoBehaviour
{
    
// Assiging the Text attribute.
    public Text playerNameText_1;
    public Text playerKillsText_1;
    public Text playerDeathsText_1;
    //public Text playerWonText_1;

    public Text playerNameText_2;
    public Text playerKillsText_2;
    public Text playerDeathsText_2;
    //public Text playerWonText_2;
    internal bool _activeSelf;

   PlayerDetail playerdetail = new PlayerDetail();
       
     public static string playerName;
     public static int playerKills;
     public static int playerDeaths;
     //public static int playerWon;

// This methode update player 1 detail print on Scoreboard.
     public void UpdatePlayerDetail_1()
    { 
      playerNameText_1.text = playerdetail.playerName;
      playerKillsText_1.text = playerdetail.kills.ToString();
      playerDeathsText_1.text = playerdetail.deaths.ToString();
      //playerWonText_1.text = playerdetail.won.ToString();
    }

// This methode update player 2 detail print on Scoreboard.
    public void UpdatePlayerDetail_2()
    { 
      playerNameText_2.text = playerdetail.playerName;
      playerKillsText_2 .text = playerdetail.kills.ToString();
      playerDeathsText_2.text = playerdetail.deaths.ToString();
      //playerWonText_2.text = playerdetail.won.ToString();
    }
    private void Start(){
      this._activeSelf = false;
      transform.gameObject.SetActive(this._activeSelf);
    }
    public void GetScoreboard(){
      this._activeSelf = !_activeSelf;
      transform.gameObject.SetActive(this._activeSelf);
      return;
    }
   private void Update()
   {
     RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player1" +".json").Then(response =>
         {
             playerdetail = response;
             UpdatePlayerDetail_1();
        });

        RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player2" +".json").Then(response =>
         {
             playerdetail = response;
             UpdatePlayerDetail_2();
        });
   }

}
