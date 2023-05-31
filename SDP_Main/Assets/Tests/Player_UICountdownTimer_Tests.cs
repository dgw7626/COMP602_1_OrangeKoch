/*

 ************************************************
 *                                              *
 * Primary Dev: 	Dion Hemmes		            *
 * Student ID: 		21154191		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *
 ************************************************

 */
using NUnit.Framework;
using UnityEngine;
using TMPro;

/// <summary>
/// Test Class to confirm correct functionality of the Player UI Countdown Timer Color changes
/// </summary>
public class Player_UICountdownTimer_Tests
{
    private Player_UIManager playerUIManager;
    private GameObject gameObject;
    private GameObject tMProUIGameObject;
    private TextMeshProUGUI timerText;
    public Color orangeColor;

    /// <summary>
    /// Method to create required objects in preparation for the Tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        playerUIManager = gameObject.AddComponent<Player_UIManager>();
        tMProUIGameObject = new GameObject();
        timerText = tMProUIGameObject.AddComponent<TextMeshProUGUI>();
        playerUIManager.timerText = timerText;
        orangeColor = new Color(1f, 0.65f, 0f); //Create orange as it does not exist by default
}


    /// <summary>
    /// Method to remove all objects created during the Setup and throughout the Tests.
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gameObject);
    }

    /// <summary>
    /// Test method that checks the Set Color method which would be used within future tests ensuring colour changing
    /// </summary>
    [Test]
    public void SetTimerColorValidColorTest()
    {
        // ARRANGE
        Color initialTimerColor = Color.green;
        playerUIManager.timerText.color = initialTimerColor;

        // ACT
        playerUIManager.SetTimerColor(Color.white);

        // ASSERT
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.white, playerUIManager.timerText.color);
    }

    /// <summary>
    /// Test method that checks the change of colour to YellowAlert for the countdown timer based on the time input
    /// </summary>
    [Test]

    public void TimerColorDeciderYellowAlertTest()
    {
        // ARRANGE
        Color initialTimerColor = Color.green;
        playerUIManager.timerText.color = initialTimerColor;
        int gameTime = 32;

        // ACT & ASSERT
        //Test colour when timer is 32 seconds (colour should be green)
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.green, playerUIManager.timerText.color);

        //Test colour when timer is 31 seconds (colour should be green)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.green, playerUIManager.timerText.color);

        //Test colour when timer is 30 seconds (colour should be yellow)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.yellow, playerUIManager.timerText.color);
        Assert.IsTrue(playerUIManager.yellowAlertThreshold >= gameTime && gameTime > playerUIManager.orangeAlertThreshold);

        //Test colour when timer is 29 seconds (colour should be yellow)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.yellow, playerUIManager.timerText.color);
        Assert.IsTrue(playerUIManager.yellowAlertThreshold >= gameTime && gameTime > playerUIManager.orangeAlertThreshold);
    }


    /// <summary>
    /// Test method that checks the change of colour to OrangeAlert for the countdown timer based on the time input
    /// </summary>
    [Test]
    public void TimerColorDeciderOrangeAlertTest()
    {
        // ARRANGE
        Color initialTimerColor = Color.yellow;
        playerUIManager.timerText.color = initialTimerColor;
        int gameTime = 17;

        // ACT & ASSERT

        Assert.AreEqual(Color.yellow, playerUIManager.timerText.color); playerUIManager.TimerColorDecider(gameTime);
        Assert.AreEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.yellow, playerUIManager.timerText.color);

        //Test colour when timer is 16 seconds (colour should be yellow)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreEqual(initialTimerColor, playerUIManager.timerText.color);

        //Test colour when timer is 15 seconds (colour should be orange)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(orangeColor, playerUIManager.timerText.color);
        Assert.AreEqual(playerUIManager.orangeColor, playerUIManager.timerText.color);
        Assert.IsTrue(playerUIManager.orangeAlertThreshold >= gameTime && gameTime > playerUIManager.redAlertThreshold);

        //Test colour when timer is 14 seconds (colour should be orange)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(orangeColor, playerUIManager.timerText.color);
        Assert.AreEqual(playerUIManager.orangeColor, playerUIManager.timerText.color);
        Assert.IsTrue(playerUIManager.orangeAlertThreshold >= gameTime && gameTime > playerUIManager.redAlertThreshold);
    }

    /// <summary>
    /// Test method that checks the change of colour to RedAlert for the countdown timer based on the time input
    /// </summary>
    [Test]
    public void TimerColorDeciderRedAlertTest()
    {
        // ARRANGE
        Color initialTimerColor = playerUIManager.orangeColor;
        playerUIManager.timerText.color = initialTimerColor;
        int gameTime = 7;

        // ACT & ASSERT
        //Test colour when timer is 7 seconds (colour should be orange)
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(orangeColor, playerUIManager.timerText.color);
        Assert.IsFalse(playerUIManager.isRedAlert);

        //Test colour when timer is 6 seconds (colour should be orange)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(orangeColor, playerUIManager.timerText.color);
        Assert.IsFalse(playerUIManager.isRedAlert);

        //Test colour when timer is 5 seconds (colour should be red)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.red, playerUIManager.timerText.color);
        Assert.IsTrue(playerUIManager.redAlertThreshold >= gameTime && gameTime > 0);
        Assert.IsTrue(playerUIManager.isRedAlert);

        //Test colour when timer is 4 seconds (colour should be red)
        gameTime--;
        playerUIManager.TimerColorDecider(gameTime);
        Assert.AreNotEqual(initialTimerColor, playerUIManager.timerText.color);
        Assert.AreEqual(Color.red, playerUIManager.timerText.color);
        Assert.IsTrue(playerUIManager.redAlertThreshold >= gameTime && gameTime > 0);
        Assert.IsTrue(playerUIManager.isRedAlert);
    }

}
