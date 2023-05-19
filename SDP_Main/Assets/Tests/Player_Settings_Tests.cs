using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Player_Settings_Tests
{
    private Player_Settings playerSettings;

    [SetUp]
    public void Setup()
    {
        playerSettings = new GameObject().AddComponent<Player_Settings>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerSettings.gameObject);
    }

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

    [Test]
    public void ChangeMouseMovementSpeedTest()
    {
        // ARRANGE
        int initialMouseMovementSpeed = 200;

        // ACT
        playerSettings.setMouseSpeed(initialMouseMovementSpeed);
        playerSettings.setMouseSpeed(100);

        // ASSERT
        Assert.AreEqual(100, playerSettings.mouseMovementSpeed);
        Assert.AreNotEqual(initialMouseMovementSpeed, playerSettings.mouseMovementSpeed);
    }

    [Test]
    public void ChangeVolumeTest()
    {
        // ARRANGE
        float initialGlobalVolume = playerSettings.globalVolume;

        // ACT
        playerSettings.setGlobalVolume(50);

        // ASSERT
        Assert.AreEqual(50, playerSettings.globalVolume);
        Assert.AreNotEqual(initialGlobalVolume, playerSettings.globalVolume);
    }

}
