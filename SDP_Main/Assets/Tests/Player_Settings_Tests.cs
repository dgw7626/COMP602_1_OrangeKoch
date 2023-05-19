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
        //  playerSettings = new GameObject().AddComponent<Player_Settings>();
    }

    [TearDown]
    public void Teardown()
    {
        //Object.DestroyImmediate(playerSettings.gameObject);
    }

    [Test]
    public void TrialTest()
    {
        // ARRANGE
        int a = 25;
        int b = 4;
        int c = 100;

        // ACT
        int calc = (a * b);

        // ASSERT
        Assert.That(calc, Is.EqualTo(c));
    }

    [Test]
    public void invertMouseYAxisTest()
    {
        // ARRANGE
        bool initialInvertMouseYAxis = playerSettings.invertMouseYAxis;

        // ACT
        playerSettings.InvertMouseYAxis();


        // ASSERT
        Assert.AreEqual(!initialInvertMouseYAxis, playerSettings.invertMouseYAxis);
    }

    [Test]
    public void ChangeMouseMovementSpeedTest()
    {
        // ARRANGE
        int initialMouseMovementSpeed = playerSettings.mouseMovementSpeed;

        // ACT
        playerSettings.setMouseSpeed(100);

        // ASSERT
        Assert.AreEqual(100, playerSettings.mouseMovementSpeed);
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
    }
}
