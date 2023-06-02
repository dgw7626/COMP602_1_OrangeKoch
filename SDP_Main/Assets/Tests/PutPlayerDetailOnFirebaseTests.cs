using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Proyecto26;
using UnityEngine;
using UnityEngine.TestTools;

public class PutPlayerDetailOnFirebaseTests
{
    public PutPlayerDetailOnFirebase playerDetailOnFirebase;

    [SetUp]
    public void Setup()
    {
     playerDetailOnFirebase = new PutPlayerDetailOnFirebase();
     GameObject playerObject = new GameObject();
     playerDetailOnFirebase = playerObject.AddComponent<PutPlayerDetailOnFirebase>();
        
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerDetailOnFirebase);
    }

    [Test]
    public void Team1Player1Detail_PutsPlayerDetailOnFirebase()
    {
        // Arrange
        string playerName = "Joe";
        int kills = 2;
        int deaths = 10;

        // Act
        playerDetailOnFirebase.team_1player_1Detail(playerName, kills, deaths);

        // Assert
        RestClient.Get<ScoreManager>("https://project-10bbb-default-rtdb.firebaseio.com/player2.json")
       .Then(response =>
       {
           Assert.IsNotNull(response); // Check if the response is not null
           Assert.AreEqual(playerName, response.playerName); // Check if the player name matches
           Assert.AreEqual(kills, response.kills); // Check if the kills match
           Assert.AreEqual(deaths, response.deaths); // Check if the deaths match
       })
       .Catch(error =>
       {
           Debug.LogError("Error retrieving player details from Firebase: " + error.Message);
       });
    }

    [Test]
    public void Team2Player3Detail_PutsPlayerDetailOnFirebase()
    {
        // Arrange
        string playerName = "Jane";
        int kills = 5;
        int deaths = 3;

        // Act
        playerDetailOnFirebase.team_2player_3Detail(playerName, kills, deaths);

        // Assert
        RestClient.Get<ScoreManager>("https://project-10bbb-default-rtdb.firebaseio.com/player7.json")
       .Then(response =>
       {
           Assert.IsNotNull(response); // Check if the response is not null
           Assert.AreEqual(playerName, response.playerName); // Check if the player name matches
           Assert.AreEqual(kills, response.kills); // Check if the kills match
           Assert.AreEqual(deaths, response.deaths); // Check if the deaths match
       })
       .Catch(error =>
       {
           Debug.LogError("Error retrieving player details from Firebase: " + error.Message);
       });
    }

    [Test]
    public void WinningTeamName_PutsWinningTeamNameOnFirebase()
    {
        // Arrange
        int winningTeamName = 1;

        // Act
        playerDetailOnFirebase.winningTeamName(winningTeamName);

        // Assert
        RestClient.Get<ScoreManager>("https://project-10bbb-default-rtdb.firebaseio.com/player7.json")
       .Then(response =>
       {
           Assert.IsNotNull(response); // Check if the response is not null
           Assert.AreEqual(winningTeamName, response.winningTeamName); // Check if the winning team name matches
       })
       .Catch(error =>
       {
           Debug.LogError("Error retrieving winning team name from Firebase: " + error.Message);
       });
    }

}
