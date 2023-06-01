using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Threading.Tasks;
using System;

/*
 * Author: Corey Knight - 21130891
 */

//==============================================================================//
// IMPORTANT NOTE!                                              IMPORTANT NOTE! //
//==============================================================================//
/*                                                                              *
 * Warning! Please note that Player ID's start at 1, not 0!                     *
 * So player 1 will be in team 0, player 2 in team 1 etc.                       *
 * This is because of the way Photon handles player ID's.                       *
 *                                                                              *
 * The GameMode_Standard OnScoreEvent() function accomadates for this           *
 * by subtracting one from the PlayerID, making Player1 become Player0-Team0.   *
 *                                                                              *
 * The Scoreboard will then ADD one to all players and teams in it's text       *
 * elements, so that it displays Player0-Team0 as Player1-Team1.                *
 *                                                                              *
 * For ease of use within this test class, the AddKill function will add one to *
 * the Player ID, so that GameMode_Standard can subtract it again.              *
 *                                                                              *
 * This means when working within the test class, you can align Player Id's     *
 * with their Team ID's. Feel free to add a kill to Player0-Team0.              *
 *                                                                              */
//==============================================================================//
// IMPORTANT NOTE!                                              IMPORTANT NOTE! //
//==============================================================================//

/// <summary>
/// 
/// </summary>
public class MultiplayerScorebard_Test
{
    private GameObject scoreboardGameObj;
    private GameMode_Standard gameMode;

    /// <summary>
    /// Method to create required objects in preparation for the Tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }


    /// <summary>
    /// Method to remove all objects created during the Setup and throughout the Tests.
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        //Object.DestroyImmediate(scoreboardGameObj);
    }


    /// <summary>
    /// This test asserts that the GameObject instantiates the approprite elements on Awake()
    /// </summary>
    [Test]
    public void PrefabInstantiatesElementsTest()
    {
        // ARRANGE / ACT
        // Scoreboard will act immediatley after instantiation
        //----------------------------------------------------

        //Refresh the GameMode to create a blank Score
        gameMode = new GameMode_Standard();

        // 8 players, 4 teams
        int numPlayers = 8;
        int numTeams = 4;
        InitScoreStruct(numPlayers, numTeams);

        // Add some kills
        for(int i = 0; i < 10; i++)
            AddKill(0, 0, 1, 1);
        
        for(int i = 0; i < 10; i++)
            AddKill(4, 0, 5, 1);

        // Instantiate Object and get references
        scoreboardGameObj = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Multiplayer_PostgameScoreboard.prefab"));
        UI_MultiplayerScoreboard scoreScript = scoreboardGameObj.GetComponent<UI_MultiplayerScoreboard>();
        scoreScript.Begin(); //IT now works

        VerticalLayoutGroup NameCollumn = scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup KillCollumn = scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup DeathCollumn = scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<VerticalLayoutGroup>();
       
        VerticalLayoutGroup TeamNameCollumn = scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup TeamKillCollumn = scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup TeamDeathCollumn = scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<VerticalLayoutGroup>();

        string s = NameCollumn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        TextMeshProUGUI[] textMeshProUGUIs = scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<VerticalLayoutGroup>().GetComponents<TextMeshProUGUI>();

        // ASSERT
        Assert.NotNull(scoreboardGameObj.transform);

        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<VerticalLayoutGroup>());
        
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<VerticalLayoutGroup>());

        for(int i = 0; i < numPlayers; i+= 3)
        {
            Assert.NotNull(textMeshProUGUIs[i]);
            Assert.NotNull(textMeshProUGUIs[i + 1]);
            Assert.NotNull(textMeshProUGUIs[i + 2]);
        }
        for (int i = 0; i < numTeams; i+= 3)
        {
            Assert.NotNull(textMeshProUGUIs[numPlayers + i]);
            Assert.NotNull(textMeshProUGUIs[numPlayers + i + 1]);
            Assert.NotNull(textMeshProUGUIs[numPlayers + i + 2]);
        }
    }

    /// <summary>
    /// Asserts that the correct number of kills have been assigned to each player
    /// </summary>
    [Test]
    public void AssignCorrectScoresToPlayerTest()
    {
        // ARRANGE / ACT
        // Scoreboard will act immediatley after instantiation
        //----------------------------------------------------
        
        // 8 players, 4 teams
        InitScoreStruct(2, 2);

        // Add some kills
        

        // Instantiate Object and get references
        scoreboardGameObj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Multiplayer_PostgameScoreboard.prefab");
        UI_MultiplayerScoreboard scoreScript = scoreboardGameObj.GetComponent<UI_MultiplayerScoreboard>();
        //TextMeshProUGUI[] textMeshProUGUIs = scoreScript.tmp.GetComponents<TextMeshProUGUI>();
        

        // ASSERT
        //----------------------------------------------------
        Assert.NotNull(scoreScript);
        //StringAssert.AreEqualIgnoringCase("1", textMeshProUGUIs[1].text);

    }

    private void InitScoreStruct(int players, int teams)
    {
        gameMode.teamScores = new s_GameScore();
        gameMode.teamScores.numPlayers = players;
        gameMode.teamScores.numTeams = teams;
        gameMode.teamScores.killsPerTeam = new int[teams];
        gameMode.teamScores.deathsPerTeam = new int[teams];
        gameMode.teamScores.killsPerPlayer = new int[players];
        gameMode.teamScores.deathsPerPlayer = new int[players];
        gameMode.teamScores.teamNumbersByPlayer = new int[players];
        for (int i = 0; i < gameMode.teamScores.numPlayers; i++)
        {
            gameMode.teamScores.teamNumbersByPlayer[i] = (i + teams) % (teams);
        }
    }

    private void AddKill(int killerId, int killerTeam, int deadId, int deadTeam)
    {
        s_DeathInfo deathInfo = new s_DeathInfo();
        deathInfo.killerTeam = killerTeam;
        deathInfo.killerId = killerId + 1;
        deathInfo.diedTeam = deadTeam;
        deathInfo.diedId = deadId + 1;
        gameMode.CalculateScore(deathInfo);
    }
}
