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

    [Test]
    public void ChangeVolumeTest()
    {
        // ARRANGE
        float initialGlobalVolume = playerSettings.globalVolume;

        // ACT
        playerSettings.setGlobalVolume(0.5f);

        // ASSERT
        Assert.AreEqual(0.5f, playerSettings.globalVolume);
        Assert.AreNotEqual(initialGlobalVolume, playerSettings.globalVolume);
    }

}
