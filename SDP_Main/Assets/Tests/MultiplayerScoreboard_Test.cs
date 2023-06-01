using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class MultiplayerScorebard_Test
{
    private s_GameScore score;
    private GameObject scoreboardGameObj;

    /// <summary>
    /// Method to create required objects in preparation for the Tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        score = new s_GameScore();
        
    }


    /// <summary>
    /// Method to remove all objects created during the Setup and throughout the Tests.
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        //Object.DestroyImmediate(scoreboardGameObj);
    }


    [Test]
    public void PrefabInstantiatesElementsTest()
    {
        // ARRANGE
        scoreboardGameObj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Multiplayer_PostgameScoreboard.prefab");

        // ASSERT
        Assert.NotNull(scoreboardGameObj.transform);

        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<VerticalLayoutGroup>());
        
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<VerticalLayoutGroup>());
        Assert.NotNull(scoreboardGameObj.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<VerticalLayoutGroup>());

    }

    /// <summary>
    /// </summary>
    [Test]
    public void AssignCorrectScoresToPlayerTest()
    {
        // ARRANGE / ACT
        // Scoreboard will act immediatley after instantiation
        //----------------------------------------------------
        
        //8 players, 4 teams
        InitScoreStruct(8, 4);
     
        // Instantiate Object and get references
        scoreboardGameObj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Multiplayer_PostgameScoreboard.prefab");
        UI_MultiplayerScoreboard scoreScript = scoreboardGameObj.GetComponent<UI_MultiplayerScoreboard>();
        TextMeshProUGUI[] textMeshProUGUIs = scoreScript.tmp.GetComponents<TextMeshProUGUI>();
        

        // ASSERT
        //----------------------------------------------------
        Assert.NotNull(scoreScript);
        StringAssert.AreEqualIgnoringCase("", textMeshProUGUIs[0].text);

    }

    private void InitScoreStruct(int players, int teams)
    {
        score.numPlayers = players;
        score.numTeams = teams;
        score.killsPerTeam = new int[teams];
        score.deathsPerTeam = new int[teams];
        score.killsPerPlayer = new int[players];
        score.deathsPerPlayer = new int[players];
        score.teamNumbersByPlayer = new int[players];
        for (int i = 0; i < score.numPlayers; i++)
        {
            score.teamNumbersByPlayer[i] = (i + teams) % (teams);
        }

        Game_RuntimeData.gameScore = score;

    }
}
