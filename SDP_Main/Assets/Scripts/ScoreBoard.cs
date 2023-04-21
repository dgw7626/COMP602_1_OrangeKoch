using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Proyecto26;
using System;
using System.Net.Mime;

public class ScoreBoard : MonoBehaviour
{
    public Text playerNameText;
    public Text playerKillsText;
    public Text playerDeathsText;
    public Text playerWonText;
    
   PlayerDetail playerdetail = new PlayerDetail();
       

        public static string playerName;
        public static int playerKills;
        public static int playerDeaths;
        public static int playerWon;


     public void UpdatePlayerDetail()
    { 
      playerNameText.text = playerdetail.playerName;
      playerKillsText.text = playerdetail.kills.ToString();
      playerDeathsText.text = playerdetail.deaths.ToString();
      playerWonText.text = playerdetail.won.ToString();
    }

   private void Start() 
   {
        RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player" + ".json").Then(response =>
         {
             playerdetail = response;
             UpdatePlayerDetail();
             
        });
   }

}
