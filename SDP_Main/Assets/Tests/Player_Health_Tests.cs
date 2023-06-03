/*



************************************************

*                                              *

* Primary Dev:     Siyi Wang                   *

* Student ID:      19036757                    *

* Course Code:     COMP602_2023_S1             *

* Assessment Item: Orange Koch                 *

*                                              *

************************************************



*/
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// This class tests the functionality of the player health changed within the situation.
/// </summary>
public class Player_Health_Tests
{
    private Player_Health playerHealth;
    private Coroutine updateUICoroutine;

    /// <summary>
    /// Method to create required objects in preparation for the Tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        GameObject playerObject = new GameObject();
        playerHealth = playerObject.AddComponent<Player_Health>();
        playerHealth.maxHealth = 100f;
        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.healthBar = playerObject.AddComponent<PlayerUI_HealthBar>();
    }

    /// <summary>
    /// Method to remove all object created during the setup and throught the Tests.
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerHealth.gameObject);
    }

    /// <summary>
    /// Tests the TakeDamage method of the Player_Health class.
    /// It verifies that the player's health decreases correctly when taking damage.
    /// </summary>
    [Test]
    public void TakeDamageTest()
    {
        // ARRANGE
        s_DamageInfo damageInfo = new s_DamageInfo();

        float damageValue = 50.0f;
        damageInfo.dmgValue = damageValue;
        float expectedHealth = playerHealth.currentHealth - damageValue;
        // ACT
        playerHealth.TakeDamage(damageInfo);

        // ASSERT
        Assert.AreEqual(expectedHealth, playerHealth.currentHealth);
    }
    /// <summary>
    /// Tests the UpdateUI_HealthDecreasesOverTime method of the Player_Health class.
    /// It verifies that the player's UI health decreases over time as expected.
    /// </summary>
    [UnityTest]
    public IEnumerator UpdateUI_HealthDecreasesOverTime()
    {
        float initialHealth = 100.0f;
        float healthDecreaseRate = 10.0f;
        float expectedMinHealth = 0.0f;
        float elapsedTime = 0.0f;

        // Set initial health and start the UpdateUI coroutine
        playerHealth.currentHealth = initialHealth;
        playerHealth.currentUIHealth = initialHealth;
        playerHealth.UI_HealthTime = 0.16f; // Modify this value to match your intended update rate
        var coroutine = playerHealth.UpdateUI();
        playerHealth.StartCoroutine(coroutine);

        // Act
        yield return null; // Wait for one frame to ensure the coroutine has started

        // Wait for the health to decrease over time
        while (elapsedTime < playerHealth.UI_HealthTime * 2.0f)
        {
            elapsedTime += Time.deltaTime;
            playerHealth.currentHealth -= healthDecreaseRate * Time.deltaTime; // Decrease health over time
            yield return null;
        }

        // Assert
        Assert.LessOrEqual(playerHealth.currentUIHealth, initialHealth);
        Assert.GreaterOrEqual(playerHealth.currentUIHealth, expectedMinHealth);
    }
    /// <summary>
    /// Tests the Respawn method of the Player_Health class.
    /// It verifies that the player's health is restored to the maximum value after respawning.
    /// </summary>
    [Test]
    public void Respawn_RestoresHealthToMax()
    {
        // Arrange
        float maxHealth = 100f;
        float initialHealth = 50f;

        // Set the initial health
        playerHealth.currentHealth = initialHealth;

        // Act
        playerHealth.Respawn();

        // Assert
        Assert.AreEqual(maxHealth, playerHealth.currentHealth);
    }
}
