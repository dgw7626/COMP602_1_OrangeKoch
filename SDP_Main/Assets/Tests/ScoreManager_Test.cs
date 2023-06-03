using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreManager_Test
{
    [Test]
    public void InitializedWithPlayerDetails()
    {
        // Arrange
        string playerName = "Joe";
        int kills = 2;
        int deaths = 10;

        // Act
        ScoreManager scoreManager = new ScoreManager(playerName, kills, deaths);

        // Assert
        Assert.AreEqual(playerName, scoreManager.playerName);
        Assert.AreEqual(kills, scoreManager.kills);
        Assert.AreEqual(deaths, scoreManager.deaths);
    }

    [Test]
    public void SInitializedWithWinningTeamName()
    {
        // Arrange
        int winningTeamName = 1;

        // Act
        ScoreManager scoreManager = new ScoreManager(winningTeamName);

        // Assert
        Assert.AreEqual(winningTeamName, scoreManager.winningTeamName);
    }
}
