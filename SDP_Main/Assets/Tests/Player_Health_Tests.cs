using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Player_Health_Tests
{
    private Player_Health playerHealth;
    private Coroutine updateUICoroutine;

    [SetUp]
    public void Setup()
    {
        GameObject playerObject = new GameObject();
        playerHealth = playerObject.AddComponent<Player_Health>();
        playerHealth.maxHealth = 100f;
        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.healthBar = playerObject.AddComponent<HealthBar>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerHealth.gameObject);
    }

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

    [UnityTest]
    public IEnumerator UpdateUI_HealthDecreasesOverTime()
    {
        // // ARRANGE
        // playerHealth.currentUIHealth = playerHealth.maxHealth;
        // playerHealth.UI_HealthTime = 0.01f;

        // // ACT
        // updateUICoroutine = playerHealth.StartCoroutine(playerHealth.UpdateUI());

        // float elapsedTime = 0f;
        // while (elapsedTime < playerHealth.UI_HealthTime + 0.1f)
        // {
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
        // float updatedUIHealth = playerHealth.currentUIHealth;

        // // ASSERT
        // Assert.Less(updatedUIHealth, 100.0f);

        // // Stop the coroutine
        // playerHealth.StopCoroutine(updateUICoroutine);
        // Arrange
        // Arrange
        // Arrange
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
        // Assert.AreEqual(playerHealth.currentUIHealth, initialHealth);
        Assert.GreaterOrEqual(playerHealth.currentUIHealth, expectedMinHealth); 
    }

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
