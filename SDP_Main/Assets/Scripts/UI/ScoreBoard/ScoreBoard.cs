/*

 ************************************************
 *                                              *				
 * Primary Dev: 	Munish Kumar                *
 * Student ID: 		19083476		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *			
 ************************************************

 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Proyecto26;
using System;
using System.Net.Mime;
using TMPro;
using UnityEngine.UIElements;

public class ScoreBoard : MonoBehaviour
{
// Assiging the Text attribute.
    public TMP_Text playerNameText_1;
    public TMP_Text playerKillsText_1;
    public TMP_Text playerDeathsText_1;
    
    public TMP_Text playerNameText_2;
    public TMP_Text playerKillsText_2;
    public TMP_Text playerDeathsText_2;

    public TMP_Text playerNameText_3;
    public TMP_Text playerKillsText_3;
    public TMP_Text playerDeathsText_3;

    public TMP_Text playerNameText_4;
    public TMP_Text playerKillsText_4;
    public TMP_Text playerDeathsText_4;

    public TMP_Text playerNameText_5;
    public TMP_Text playerKillsText_5;
    public TMP_Text playerDeathsText_5;

    public TMP_Text playerNameText_6;
    public TMP_Text playerKillsText_6;
    public TMP_Text playerDeathsText_6;

    internal bool _activeSelf;

   PlayerDetail playerdetail = new PlayerDetail();
       
     public static string playerName;
     public static int playerKills;
     public static int playerDeaths;

/// <summary>
/// This methode update player 1 detail print on Scoreboard.
/// </summary>
     public void UpdatePlayerDetail_1()
    { 
      playerNameText_1.text = playerdetail.playerName;
      playerKillsText_1.text = playerdetail.kills.ToString();
      playerDeathsText_1.text = playerdetail.deaths.ToString();
    }

/// <summary>
/// This methode update player 2 detail print on Scoreboard.
/// </summary>
    public void UpdatePlayerDetail_2()
    { 
      playerNameText_2.text = playerdetail.playerName;
      playerKillsText_2 .text = playerdetail.kills.ToString();
      playerDeathsText_2.text = playerdetail.deaths.ToString();
    }

    /// <summary>
    /// This methode update player 3 detail print on Scoreboard.
    /// </summary>
    public void UpdatePlayerDetail_3()
    {
        playerNameText_3.text = playerdetail.playerName;
        playerKillsText_3.text = playerdetail.kills.ToString();
        playerDeathsText_3.text = playerdetail.deaths.ToString();
    }

    /// <summary>
    /// This methode update player 4 detail print on Scoreboard.
    /// </summary>
    public void UpdatePlayerDetail_4()
    {
        playerNameText_4.text = playerdetail.playerName;
        playerKillsText_4.text = playerdetail.kills.ToString();
        playerDeathsText_4.text = playerdetail.deaths.ToString();
    }

    /// <summary>
    /// This methode update player 5 detail print on Scoreboard.
    /// </summary>
    public void UpdatePlayerDetail_5()
    {
        playerNameText_5.text = playerdetail.playerName;
        playerKillsText_5.text = playerdetail.kills.ToString();
        playerDeathsText_5.text = playerdetail.deaths.ToString();
    }

    /// <summary>
    /// This methode update player 6 detail print on Scoreboard.
    /// </summary>
    public void UpdatePlayerDetail_6()
    {
        playerNameText_6.text = playerdetail.playerName;
        playerKillsText_6.text = playerdetail.kills.ToString();
        playerDeathsText_6.text = playerdetail.deaths.ToString();
    }

    /// <summary>
    /// This method disable the game object of each UI element
    /// </summary>
    private void Start(){
      this._activeSelf = false;
        StartCoroutine(GetDetails());
        foreach (Transform child in transform)
        {
            // Disable the GameObject of each UI element
            child.gameObject.SetActive(_activeSelf);
        }
    }

    /// <summary>
    /// This method show scoreboard on main game room.
    /// </summary>
    public void GetScoreboard(){
      this._activeSelf = !_activeSelf;
        foreach (Transform child in transform)
        {
            // Disable the GameObject of each UI element
            child.gameObject.SetActive(_activeSelf);
        }
        GetDetails();
      return;
    }

    /// <summary>
    /// This methode get player data from firebase and pass to updateplayerdetal method.
    /// </summary>
    /// <returns> It return the time 1.1sec </returns>
    private IEnumerator GetDetails()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.1f);
            RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player1" +".json").Then(response =>
                {
                    playerdetail = response;
                    UpdatePlayerDetail_1();
            });

            RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player2" +".json").Then(response =>
                {
                    playerdetail = response;
                    UpdatePlayerDetail_4();
                });

            RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player3" + ".json").Then(response =>
            {
                playerdetail = response;
                UpdatePlayerDetail_2();
            });

            RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player4" + ".json").Then(response =>
            {
                playerdetail = response;
                UpdatePlayerDetail_5();
            });

            RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player5" + ".json").Then(response =>
            {
                playerdetail = response;
                UpdatePlayerDetail_3();
            });

            RestClient.Get<PlayerDetail>("https://project-10bbb-default-rtdb.firebaseio.com/" + "/player6" + ".json").Then(response =>
            {
                playerdetail = response;
                UpdatePlayerDetail_6();
            });

        }
    }

}
