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

/// <summary>
/// This class tests the functionality of the player configuration settings within the options menu.
/// </summary>
public class Player_Settings_Tests
{
    private Player_Settings playerSettings;

    /// <summary>
    /// Method to create required objects in preparation for the Tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        playerSettings = new Player_Settings();
    }


    /// <summary>
    /// Method to remove all objects created during the Setup and throughout the Tests.
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        playerSettings = null;
    }

    /// <summary>
    /// Test method to check Inversion of the Mouse Y Axis.
    /// </summary>
    [Test]
    public void InvertMouseYAxisTest()
    {
        // ARRANGE
        bool initialInvertMouseYAxis = playerSettings.invertMouseYAxis;

        // ACT
        playerSettings.InvertMouseYAxis();


        // ASSERT
        Assert.AreNotEqual(initialInvertMouseYAxis, playerSettings.invertMouseYAxis);
    }

    /// <summary>
    /// Method to Set the Mouse Y Axis Inversion setting for the player.
    /// </summary>
    [Test]

    public void SetInvertMouseYAxisTest()
    {
        // ARRANGE
        bool initialInvertMouseYAxis = playerSettings.invertMouseYAxis;

        // ACT
        playerSettings.setInvertMouseYAxis(true);

        // ASSERT
        Assert.AreEqual(true, playerSettings.invertMouseYAxis);
        Assert.AreNotEqual(initialInvertMouseYAxis, playerSettings.invertMouseYAxis);
    }


    /// <summary>
    /// Method to Set the Look Sensitivity setting within the bounds for the player.
    /// </summary>
    [Test]
    public void SetLookSensitivityInsideBoundsTest()
    {
        // ARRANGE
        float initialLookSensitivityMultiplier = 2f;

        // ACT
        playerSettings.setMouseSpeed(initialLookSensitivityMultiplier);
        playerSettings.setMouseSpeed(1f);

        // ASSERT
        Assert.AreEqual(1f, playerSettings.lookSensitivity);
        Assert.AreNotEqual(initialLookSensitivityMultiplier, playerSettings.lookSensitivity);
    }

    /// <summary>
    /// Method to Set the Look Sensitivity setting outside the bounds for the player.
    /// </summary>
    [Test]
    public void SetLookSensitivityOutsideBoundsTest()
    {
        // ARRANGE
        float initialLookSensitivityMultiplier = 2f;

        // ACT
        playerSettings.setMouseSpeed(initialLookSensitivityMultiplier);
        playerSettings.setMouseSpeed(0.1f);

        // ASSERT
        Assert.AreEqual(initialLookSensitivityMultiplier, playerSettings.lookSensitivity);
    }

    /// <summary>
    /// Method to Set the Global Volume setting for the player.
    /// </summary>
    [Test]
    public void SetValidVolumeTest()
    {
        // ARRANGE
        float initialGlobalVolume = playerSettings.globalVolume;

        // ACT
        playerSettings.setGlobalVolume(0.5f);

        // ASSERT
        Assert.AreEqual(0.5f, playerSettings.globalVolume);
        Assert.AreNotEqual(initialGlobalVolume, playerSettings.globalVolume);
    }

    /// <summary>
    /// Method to Set the Global Volume setting for the player.
    /// </summary>
    [Test]
    public void SetInvalidVolumeTest()
    {
        // ARRANGE
        float initialGlobalVolume = playerSettings.globalVolume;

        // ACT
        playerSettings.setGlobalVolume(-0.5f);

        // ASSERT
        Assert.AreNotEqual(-0.5f, playerSettings.globalVolume);
        Assert.AreEqual(initialGlobalVolume, playerSettings.globalVolume);
    }

}
