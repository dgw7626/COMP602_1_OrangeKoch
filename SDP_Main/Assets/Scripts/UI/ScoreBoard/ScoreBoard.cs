using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  Proyecto26;
using System;
using System.Net.Mime;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
// Assiging the Text attribute.
    public TMP_Text playerNameText_1;
    public TMP_Text playerKillsText_1;
    public TMP_Text playerDeathsText_1;
    public TMP_Text playerWonText_1;

    public TMP_Text playerNameText_2;
    public TMP_Text playerKillsText_2;
    public TMP_Text playerDeathsText_2;
    internal bool _activeSelf;

   PlayerDetail playerdetail = new PlayerDetail();
       
     public static string playerName;
     public static int playerKills;
     public static int playerDeaths;

// This methode update player 1 detail print on Scoreboard.
     public void UpdatePlayerDetail_1()
    { 
      playerNameText_1.text = playerdetail.playerName;
      playerKillsText_1.text = playerdetail.kills.ToString();
      playerDeathsText_1.text = playerdetail.deaths.ToString();
    }

// This methode update player 2 detail print on Scoreboard.
    public void UpdatePlayerDetail_2()
    { 
      playerNameText_2.text = playerdetail.playerName;
      playerKillsText_2 .text = playerdetail.kills.ToString();
      playerDeathsText_2.text = playerdetail.deaths.ToString();
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
